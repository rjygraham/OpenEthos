targetScope = 'resourceGroup'

param location string
param environmentRegionName string
param regionCoreResourceGroupName string
param apiName string
param apiDisplayName string
param apiVersions array
param serverFarmResourceId string
param appInsightsInstrumentationKey string
param aadOpenIdIssuer string
param aadApisClientId string

var apimName = '${environmentRegionName}-apim'
var funcStorageAccountName = toLower(replace('${environmentRegionName}funcstg', '-', ''))

resource apim 'Microsoft.ApiManagement/service@2021-01-01-preview' existing = {
  name: apimName
  scope: resourceGroup(regionCoreResourceGroupName)
}

resource funcStorageAccount 'Microsoft.Storage/storageAccounts@2021-02-01' = {
  name: funcStorageAccountName
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
  properties: {
    accessTier: 'Hot'
    allowBlobPublicAccess: false
    supportsHttpsTrafficOnly: true
  }
}

module apiVersionSetDeployment 'apim.versionset.bicep' = {
  name: '${apim.name}.${apimName}.versionset'
  scope: resourceGroup(regionCoreResourceGroupName)
  params: {
    apimName: apim.name
    apiName: apiName
    apiDisplayName: apiDisplayName
  }
}

module apiVersion 'api.version.bicep' = [for version in apiVersions: {
  name: '${apiName}.api.${version}'
  dependsOn: [
    apiVersionSetDeployment
  ]
  params: {
    location: location
    environmentRegionName: environmentRegionName
    apimName: apimName
    apimPricipalId: apim.identity.principalId
	  apiDisplayName: apiDisplayName
    apiName: apiName
    version: version
    serverFarmResourceId: serverFarmResourceId
    storageAccountConnectionString: 'DefaultEndpointsProtocol=https;AccountName=${funcStorageAccountName};AccountKey=${listkeys(funcStorageAccount.id, funcStorageAccount.apiVersion).keys[0].value}'
    appInsightsInstrumentationKey: appInsightsInstrumentationKey
    aadOpenIdIssuer: aadOpenIdIssuer
		aadApisClientId: aadApisClientId
  }
}]

