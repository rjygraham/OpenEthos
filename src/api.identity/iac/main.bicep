targetScope = 'subscription'

param location string
param appName string
param environment string
param regions array
param apiVersions array
param serverFarmResourceId string
param appInsightsInstrumentationKey string
param aadOpenIdIssuer string
param aadApisClientId string

var environmentName = '${appName}-${environment}'
var apiName = 'identity'
var apiDisplayName = 'Identity'
var apiResourceGroupName = toUpper('${environmentName}-${apiDisplayName}')
var regionMap = {
	eastus: 'eus'
	westus: 'wus'
}

resource apiResourceGroup 'Microsoft.Resources/resourceGroups@2020-06-01' = {
  name: apiResourceGroupName
  location: location
}

module regionDeployment 'region.bicep' = [for region in regions : {
  name: '${apiName}.${region}.deployment'
  scope: apiResourceGroup
  params: {
    location: region
    environmentRegionName: '${environmentName}-${regionMap[region]}'
    regionCoreResourceGroupName: toUpper('${environmentName}-${regionMap[region]}-CORE')
    apiName: apiName
    apiDisplayName: apiDisplayName
    apiVersions: apiVersions
    serverFarmResourceId: serverFarmResourceId
    appInsightsInstrumentationKey: appInsightsInstrumentationKey
    aadOpenIdIssuer: aadOpenIdIssuer
    aadApisClientId: aadApisClientId
  }
}]
