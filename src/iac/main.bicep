targetScope = 'subscription'

param location string
param appName string
param environment string
param regions array
param identityApiVersions array
param inboxApiVersions array
param outboxApiVersions array
param profileApiVersions array
param aadOpenIdConfigUrl string
param aadOpenIdApimAudience string
param aadOpenIdIssuer string
param b2cOpenIdConfigUrl string
param b2cOpenIdAudience string
param b2cOpenIdIssuer string
param aadApisClientId string
param idHintTokenIssuer string
param idHintTokenClientId string
param o365GraphTenantId string
param o365GraphClientId string
@secure()
param o365GraphClientSecret string
param o365GraphEmailSenderObjectId string

var environmentName = '${appName}-${environment}'
var coreResourceGroupName = toUpper('${environmentName}-CORE')
var opsResourceGroupName = toUpper('${environmentName}-OPS')
var regionMap = {
	eastus: 'eus'
	westus: 'wus'
}

resource coreResourceGroup 'Microsoft.Resources/resourceGroups@2020-06-01' = {
	name: coreResourceGroupName
	location: location
}

resource opsResourceGroup 'Microsoft.Resources/resourceGroups@2020-06-01' = {
	name: opsResourceGroupName
	location: location
}

resource regionSharedResourceGroups 'Microsoft.Resources/resourceGroups@2020-06-01' = [for region in regions: {
	name: toUpper('${environmentName}-${regionMap[region]}-CORE')
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

module coreDeployment 'core.bicep' = {
	name: 'core'
	dependsOn: [
		coreResourceGroup
	]
	scope: resourceGroup(coreResourceGroupName)
	params: {
		location: location
		environmentName: environmentName
	}
}

module regionDeployments 'region.core.bicep' = [for region in regions: {
	name: region
	dependsOn: [
		opsDeployment
		regionSharedResourceGroups
	]
	scope: resourceGroup(toUpper('${environmentName}-${regionMap[region]}-CORE'))
	params: {
		location: location
		environmentRegionName: '${environmentName}-${regionMap[region]}'
		sharedResourceGoupName: coreResourceGroupName
		opsResourceGroupName: opsResourceGroupName
		cosmosDbName: coreDeployment.outputs.cosmosDbName
		apimPublisherEmail: 'admin@openethos.io'
		apimPublisherName: 'OpenEthos'
		aadOpenIdConfigUrl: aadOpenIdConfigUrl
		aadOpenIdApimAudience: aadOpenIdApimAudience
		aadOpenIdIssuer: aadOpenIdIssuer
		b2cOpenIdConfigUrl: b2cOpenIdConfigUrl
		b2cOpenIdAudience: b2cOpenIdAudience
		b2cOpenIdIssuer: b2cOpenIdIssuer
		aadApisClientId: aadApisClientId
		idHintTokenIssuer: idHintTokenIssuer
		idHintTokenClientId: idHintTokenClientId
		o365GraphTenantId: o365GraphTenantId
		o365GraphClientId: o365GraphClientId
		o365GraphClientSecret: o365GraphClientSecret
		o365GraphEmailSenderObjectId: o365GraphEmailSenderObjectId
	}
}]
