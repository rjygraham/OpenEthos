targetScope = 'resourceGroup'

param location string
param name string
param apimName string
param apiDisplayName string
param apiName string
param apiVersions array
param serverFarmResourceId string
param storageAccountConnectionString string {
  secure: true
}

resource identityApiVersionSet 'Microsoft.ApiManagement/service/api-version-sets@2018-06-01-preview' = {
	name: '${apimName}/${apiName}'
	properties: {
		displayName: '${apiDisplayName}'
		versioningScheme: 'Header'
		versionHeaderName: 'api-version'
	}
}

module apiVersion 'api-version.bicep' = [for version in apiVersions: {
  name: '${apiName}.api.${version}'
  params: {
    location: location
    name: name
    apimName: apimName
    apiName: apiName
    version: version
    serverFarmResourceId: serverFarmResourceId
    storageAccountConnectionString: storageAccountConnectionString
  }
}]