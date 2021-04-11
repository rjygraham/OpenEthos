targetScope = 'subscription'

param location string
param appName string
param environment string
param regions array
param identityApiVersions array
param inboxApiVersions array
param outboxApiVersions array
param profileApiVersions array
param apiOpenIdIssuer string
param apiOpenIdClientId string

var environmentName = '${appName}-${environment}'
var sharedResourceGroupName = toUpper('${environmentName}-SHARED')
var opsResourceGroupName = toUpper('${environmentName}-OPS')
var regionMap = {
	eastus: 'eus'
	westus: 'wus'
}

resource sharedResourceGroup 'Microsoft.Resources/resourceGroups@2020-06-01' = {
	name: sharedResourceGroupName
	location: location
}

resource opsResourceGroup 'Microsoft.Resources/resourceGroups@2020-06-01' = {
	name: opsResourceGroupName
	location: location
}

resource regionResourceGroups 'Microsoft.Resources/resourceGroups@2020-06-01' = [for region in regions: {
	name: toUpper('${environmentName}-${regionMap[region]}')
	location: region
}]

module opsDeployment 'ops.bicep' = {
	name: 'ops'
	dependsOn: [
		opsResourceGroup
	]
	scope: resourceGroup(opsResourceGroupName)
	params: {
		location: location
		environmentName: environmentName
	}
}

module sharedDeployment 'shared.bicep' = {
	name: 'shared'
	dependsOn: [
		sharedResourceGroup
	]
	scope: resourceGroup(sharedResourceGroupName)
	params: {
		location: location
		environmentName: environmentName
	}
}

module regionDeployments 'region.bicep' = [for region in regions: {
	name: region
	dependsOn: [
		opsDeployment
		regionResourceGroups
	]
	scope: resourceGroup(toUpper('${environmentName}-${regionMap[region]}'))
	params: {
		location: location
		environmentRegionName: '${environmentName}-${regionMap[region]}'
		sharedResourceGoupName: sharedResourceGroupName
		opsResourceGroupName: opsResourceGroupName
		cosmosDbName: sharedDeployment.outputs.cosmosDbName
		logAnalyticsName: opsDeployment.outputs.logAnalyticsName
		identityApiVersions: identityApiVersions
		inboxApiVersions: inboxApiVersions
		outboxApiVersions: outboxApiVersions
		profileApiVersions: profileApiVersions
		apimPublisherEmail: 'admin@openethos.io'
		apimPublisherName: 'OpenEthos'
		apiOpenIdIssuer: apiOpenIdIssuer
		apiOpenIdClientId: apiOpenIdClientId
	}
}]
