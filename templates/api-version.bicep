targetScope = 'resourceGroup'

param location string
param environmentRegionName string
param apimName string
param apimPricipalId string
param apiDisplayName string
param apiName string
param version string
param serverFarmResourceId string
param storageAccountConnectionString string
param appInsightsInstrumentationKey string
param apiOpenIdIssuer string
param apiOpenIdClientId string

var functionAppName = '${environmentRegionName}-${apiName}-${version}-func'

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
			appSettings: [
				{
					name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
					value: appInsightsInstrumentationKey
				}
				{
					name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
					value: 'InstrumentationKey=${appInsightsInstrumentationKey};'
				}
				{
					name: 'FUNCTIONS_WORKER_RUNTIME'
					value: 'dotnet-isolated'
				}
				{
					name: 'FUNCTIONS_EXTENSION_VERSION'
					value: '~3'
				}
				{
					name: 'AzureWebJobsStorage'
					value: storageAccountConnectionString
				}
				{
					name: 'WEBSITE_MOUNT_ENABLED'
					value: '1'
				}
				{
					name: 'WEBSITE_RUN_FROM_PACKAGE'
					value: '1'
				}
				{
					name: 'NOOP_AUTHENTICATION_SECRET'
					value: ''
				}
			]
			netFrameworkVersion: 'v5.0'
			http20Enabled: true
			minTlsVersion: '1.2'
			scmMinTlsVersion: '1.2'
			ftpsState: 'Disabled'
		}
		serverFarmId: serverFarmResourceId
	}

	resource auth 'config' = {
		name: 'authsettingsV2'
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
						openIdIssuer: apiOpenIdIssuer
						clientId: apiOpenIdClientId
						clientSecretSettingName: 'NOOP_AUTHENTICATION_SECRET'
					}
					login: {
						disableWWWAuthenticate: false
					}
					validation: {
						jwtClaimChecks: {}
						allowedAudiences: [
							apiOpenIdClientId
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
}

resource hostKey 'Microsoft.Web/sites/host/functionkeys@2020-06-01' = {
	name: '${func.name}/default/apim'
	properties: {
		name: 'apim'
	}
}

resource apimBackend 'Microsoft.ApiManagement/service/backends@2020-06-01-preview' = {
	name: '${apimName}/${apiName}-${version}'
	properties: {
		protocol: 'http'
		url: 'https://${func.properties.hostNames[0]}/api'
		resourceId: 'https://management.azure.com${func.id}'
		credentials: {
			header: {
				'x-functions-key': [
					listkeys('${func.id}/host/default/', '2020-06-01').functionKeys.apim
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
