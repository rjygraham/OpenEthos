targetScope = 'resourceGroup'

param location string
param name string
param apimName string
param apiName string
param version string
param serverFarmResourceId string
param storageAccountConnectionString string {
  secure: true
}

var functionAppName = '${name}-${apimName}-${version}-func'

resource func 'Microsoft.Web/sites@2020-06-01' = {
	name: functionAppName
	location: location
	kind: 'functionapp,linux'
	identity: {
		type: 'SystemAssigned'
	}
	properties: {
		siteConfig: {
			appSettings: [
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
		}
		serverFarmId: serverFarmResourceId
	}
}

resource api 'Microsoft.ApiManagement/service/apis@2020-06-01-preview' = {
	name: '${apimName}/${apiName}-${version}'
	properties: {
		apiType: 'http'
		apiVersion: version
		apiVersionSetId: '/apiVersionSets/${apiName}'
		path: 'identity'
		displayName: version
		protocols: [
			'https'
		]
	}
}