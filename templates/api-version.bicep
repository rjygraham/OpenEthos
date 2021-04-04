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
			]
			netFrameworkVersion: 'v5.0'
			http20Enabled: true
			minTlsVersion: '1.2'
			scmMinTlsVersion: '1.2'
			ftpsState: 'Disabled'
		}
		serverFarmId: serverFarmResourceId
	}
}

resource auth 'Microsoft.Web/sites/config@2020-06-01' = {
	name: '${func.name}/authsettingsV2'
	properties: {
		platform: {
			enabled: true
			runtimeVersion: '~1'
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
				}
				login: {
					disableWWWAuthenticate: true
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
		serviceUrl: 'https://${func.properties.hostNames[0]}'
	}
}