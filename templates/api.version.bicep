targetScope = 'resourceGroup'

param location string
param environmentRegionName string
param keyVaultName string
param apimName string
param apimPricipalId string
param apiDisplayName string
param apiName string
param version string
param serverFarmResourceId string
param storageAccountConnectionString string
param appInsightsInstrumentationKey string
param aadOpenIdIssuer string
param aadApisClientId string
param websiteLoadCertificateThumbprints string = ''

var functionAppName = '${environmentRegionName}-${apiName}-${version}-func'
var defaultSettings = {
	'WEBSITE_MOUNT_ENABLED': '1'
	'WEBSITE_RUN_FROM_PACKAGE': '1'
}
var websiteLoadCertificates = empty(websiteLoadCertificateThumbprints) ? {} : {
	'WEBSITE_LOAD_CERTIFICATES': websiteLoadCertificateThumbprints
}
var constantSettings = {
	'APPINSIGHTS_INSTRUMENTATIONKEY': appInsightsInstrumentationKey
	'APPLICATIONINSIGHTS_CONNECTION_STRING': 'InstrumentationKey=${appInsightsInstrumentationKey};'
	'AzureWebJobsStorage': storageAccountConnectionString
	'FUNCTIONS_EXTENSION_VERSION': '~3'
	'FUNCTIONS_WORKER_RUNTIME': 'dotnet'
	'NOOP_AUTHENTICATION_SECRET': ''
	'AzureKeyVaultEndpoint': 'https://${keyVaultName}.vault.azure.net/'
}
var newSettings = {}

resource func 'Microsoft.Web/sites@2020-06-01' = {
	name: functionAppName
	location: location
	kind: 'functionapp,linux'
	identity: {
		type: 'SystemAssigned'
	}
	properties: {
		httpsOnly: true
		siteConfig: {
			http20Enabled: true
			minTlsVersion: '1.2'
			scmMinTlsVersion: '1.2'
			ftpsState: 'Disabled'
		}
		serverFarmId: serverFarmResourceId
	}
}

// Create symbolic resource name to get resource Id of appsettings resource.
resource existingSettings 'Microsoft.Web/sites/config@2020-10-01' existing = {
	name: '${functionAppName}/appsettings'
}

module funcAppSettingsDeployment 'api.version.appsettings.bicep' = {
	name: '${functionAppName}.appsettings'
	params: {
		functionAppName: func.name
		defaultSettings: defaultSettings
		existingSettings: list(existingSettings.id, '2020-10-01').properties
		constantSettings: union(websiteLoadCertificates, constantSettings)
		newSettings: newSettings
	}
}

resource funcAppAuthSettings 'Microsoft.Web/sites/config@2020-10-01' = {
	name: '${functionAppName}/authsettingsV2'
	dependsOn: [
		funcAppSettingsDeployment
	]
	properties: {
		platform: {
			enabled: true
		}
		globalValidation: {
			requireAuthentication: true
			unauthenticatedClientAction: 'Return403'
			redirectToProvider: 'azureactivedirectory'
		}
		identityProviders: {
			azureActiveDirectory: {
				enabled: true
				registration: {
					openIdIssuer: aadOpenIdIssuer
					clientId: aadApisClientId
					clientSecretSettingName: 'NOOP_AUTHENTICATION_SECRET'
				}
				login: {
					disableWWWAuthenticate: false
				}
				validation: {
					jwtClaimChecks: {}
					allowedAudiences: [
						aadApisClientId
					]
					allowedClientApplications: [
						apimPricipalId
					]
				}
				isAutoProvisioned: true
			}
		}
	}
}

resource apimFunctionKeyNamedValue 'Microsoft.ApiManagement/service/namedValues@2020-12-01' = {
	name: '${apimName}/${func.name}-key'
	dependsOn: [
		funcAppAuthSettings
	]
	properties: {
		displayName: '${func.name}-key'
		secret: true
		value: listkeys('${func.id}/host/default', '2020-06-01').functionKeys.default
		tags: [
			'key'
			'function'
		]
	}
}

resource apimBackend 'Microsoft.ApiManagement/service/backends@2020-06-01-preview' = {
	name: '${apimName}/${apiName}-${version}'
	dependsOn: [
		apimFunctionKeyNamedValue
	]
	properties: {
		protocol: 'http'
		url: 'https://${func.properties.hostNames[0]}/api'
		resourceId: 'https://management.azure.com${func.id}'
		credentials: {
			header: {
				'x-functions-key': [
					//listkeys('${func.id}/host/default/', '2020-06-01').functionKeys.default
					'{{${func.name}-key}}'
				]
			}
		}
	}
}

resource api 'Microsoft.ApiManagement/service/apis@2020-06-01-preview' = {
	name: '${apimName}/${apiName}-${version}'
	properties: {
		apiType: 'http'
		apiVersion: version
		apiVersionSetId: '/apiVersionSets/${apiName}'
		path: apiName
		displayName: apiDisplayName
		protocols: [
			'https'
		]
	}

	resource policy 'policies@2020-12-01' = {
		name: 'policy'
		dependsOn: [
			apimBackend
		]
		properties: {
			value: '<policies>\r\n  <inbound>\r\n    <base />\r\n    <set-backend-service backend-id="${apiName}-${version}" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>'
		}
	}
}

resource keyVaultSecretsUserRoleDefinition 'Microsoft.Authorization/roleDefinitions@2020-03-01-preview' existing = {
	name: '4633458b-17de-408a-b874-0445c86b69e6'
	scope: subscription()
}

resource keyVault 'Microsoft.KeyVault/vaults@2020-04-01-preview' existing = {
	name: keyVaultName
	scope: resourceGroup()
}

resource funcAppKeyVaultRoleAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
	name: guid(func.name, keyVault.name, 'Key Vault Secrets User')
	properties: {
		principalId: func.identity.principalId
		roleDefinitionId: keyVaultSecretsUserRoleDefinition.id
	}
	scope: keyVault
}
