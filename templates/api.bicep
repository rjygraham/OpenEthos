targetScope = 'resourceGroup'

param location string
param environmentRegionName string
param apimName string
param apimPricipalId string
param apiDisplayName string
param apiName string
param apiVersions array
param serverFarmResourceId string
param appInsightsInstrumentationKey string
param apiOpenIdIssuer string
param apiOpenIdClientId string

@secure()
param storageAccountConnectionString string

resource apiVersionSet 'Microsoft.ApiManagement/service/api-version-sets@2018-06-01-preview' = {
	name: '${apimName}/${apiName}'
	properties: {
		displayName: '${apiDisplayName}'
		versioningScheme: 'Header'
		versionHeaderName: 'api-version'
	}
}

module apiVersion 'api-version.bicep' = [for version in apiVersions: {
  name: '${apiName}.api.${version}'
  dependsOn: [
    apiVersionSet
  ]
  params: {
    location: location
    environmentRegionName: environmentRegionName
    apimName: apimName
    apimPricipalId: apimPricipalId
	  apiDisplayName: apiDisplayName
    apiName: apiName
    version: version
    serverFarmResourceId: serverFarmResourceId
    storageAccountConnectionString: storageAccountConnectionString
    appInsightsInstrumentationKey: appInsightsInstrumentationKey
    apiOpenIdIssuer: apiOpenIdIssuer
		apiOpenIdClientId: apiOpenIdClientId
  }
}]