targetScope = 'resourceGroup'

param location string
param environmentRegionName string
param apimName string
param apimPricipalId string
param apiDisplayName string
param apiName string
param version string
param serverFarmResourceId string
@secure()
param storageAccountConnectionString string
param appInsightsInstrumentationKey string
param aadOpenIdIssuer string
param aadApisClientId string

var keyVaultName = '${environmentRegionName}-kvlt'
var functionAppName = '${environmentRegionName}-${apiName}-${version}-func'
var defaultSettings = {
	'WEBSITE_MOUNT_ENABLED': '1'
	'WEBSITE_RUN_FROM_PACKAGE': '1'
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

resource func 'Microsoft.Web/sites@2020-09-01' = {
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
resource existingSettings 'Microsoft.Web/sites/config@2020-09-01' existing = {
	name: '${functionAppName}/appsettings'
}

module funcAppSettingsDeployment 'api.version.appsettings.bicep' = {
	name: '${functionAppName}.appsettings'
	params: {
		functionAppName: func.name
		defaultSettings: defaultSettings
		existingSettings: list(existingSettings.id, existingSettings.apiVersion).properties
		constantSettings: constantSettings
		newSettings: newSettings
	}
}

resource funcAppAuthSettings 'Microsoft.Web/sites/config@2020-09-01' = {
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

module apiFuncKeyVaultRbacDeployment 'api.keyvault.rbac.bicep' = {
	name: '${func.name}.kvlt.rbac.deployment'
	scope: resourceGroup('${environmentRegionName}-CORE')
	params: {
		apiFuncName: func.name
		apiFuncPrincipalId: func.identity.principalId
		keyVaultName: keyVaultName
	}
}

module apiVersionApimDeployment 'api.version.apim.bicep' = {
	name: '${func.name}.apim.deployment'
	scope: resourceGroup('${environmentRegionName}-CORE')
	params: {
		apimName: apimName
		apiName: apiName
		apiDisplayName: apiDisplayName
		version: version
		funcName: func.name
		funcId: func.id
		funcHostName: func.properties.hostNames[0]
		funcHostKey: listkeys('${func.id}/host/default', func.apiVersion).functionKeys.default
	}
}
