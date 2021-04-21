targetScope = 'resourceGroup'

param location string
param environmentRegionName string
param opsResourceGroupName string
param sharedResourceGoupName string
param cosmosDbName string
param logAnalyticsName string
param identityApiVersions array
param inboxApiVersions array
param outboxApiVersions array
param profileApiVersions array 
param apimPublisherEmail string
param apimPublisherName string
param apiOpenIdIssuer string
param apiOpenIdClientId string

var appInsightsName = '${environmentRegionName}-ai'
var serverFarmName = '${environmentRegionName}-asp'
var storageAccountName = toLower(replace('${environmentRegionName}funcstg', '-', ''))
var apimName = '${environmentRegionName}-apim'
var keyVaultName = '${environmentRegionName}-kvlt'

resource cosmosDb 'Microsoft.DocumentDB/databaseAccounts@2021-03-01-preview' existing = {
	name: cosmosDbName
	scope: resourceGroup(sharedResourceGoupName)
}

resource logAnalytics 'Microsoft.OperationalInsights/workspaces@2020-10-01' existing = {
	name: logAnalyticsName
	scope: resourceGroup(opsResourceGroupName)
}

resource serverFarm 'Microsoft.Web/serverfarms@2020-06-01' = {
	name: serverFarmName
	location: location
	kind: 'functionapp'
	properties: {
		reserved: true
	}
	sku: {
		tier: 'Dynamic'
		name: 'Y1'
	}
}

resource appInsights 'Microsoft.Insights/components@2020-02-02-preview' = {
	name: appInsightsName
	location: location
	kind: 'web'
	properties: {
		Application_Type: 'web'
		WorkspaceResourceId: logAnalytics.id
	}
}

resource stg 'Microsoft.Storage/storageAccounts@2020-08-01-preview' = {
	name: storageAccountName
	location: location
	kind: 'StorageV2'
	sku: {
		name: 'Standard_LRS'
	}
}

resource keyVault 'Microsoft.KeyVault/vaults@2019-09-01' = {
	name: keyVaultName
	location: location
	properties: {
		sku: {
			name: 'standard'
			family: 'A'
		}
		tenantId: subscription().tenantId
    accessPolicies: []
    enablePurgeProtection: true
    enableSoftDelete: true
    enableRbacAuthorization: true
	}

	resource cosmosDbConnectionStringSecret 'secrets' = {
		name: 'CosmosDbConnectionString'
		properties: {
			value: 'AccountEndpoint=https://${cosmosDb.name}.documents.azure.com:443/;AccountKey=${listkeys(cosmosDb.id, '2021-03-01-preview').primaryMasterKey};'
		}
	}

}

resource apim 'Microsoft.ApiManagement/service@2020-06-01-preview' = {
	name: apimName
	location: location
	identity: {
		type: 'SystemAssigned'
	}
	sku: {
		name: 'Consumption'
		capacity: 0
	}
	properties: {
		publisherEmail: apimPublisherEmail
		publisherName: apimPublisherName
		customProperties: {
			'Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Protocols.Tls10': 'False'
			'Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Protocols.Tls11': 'False'
			'Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Backend.Protocols.Tls10': 'False'
			'Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Backend.Protocols.Tls11': 'False'
			'Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Backend.Protocols.Ssl30': 'False'
			'Microsoft.WindowsAzure.ApiManagement.Gateway.Protocols.Server.Http2': 'True'
		}
	}

	resource policy 'policies@2020-12-01' = {
		name: 'policy'
		properties: {
			value: '<policies>\r\n  <inbound>\r\n    <authentication-managed-identity resource="${apiOpenIdClientId}" output-token-variable-name="msi-access-token" ignore-error="false" />\r\n    <set-header name="Authorization" exists-action="override">\r\n      <value>@("Bearer " + (string)context.Variables["msi-access-token"])</value>\r\n    </set-header>\r\n  </inbound>\r\n  <backend>\r\n    <forward-request />\r\n  </backend>\r\n  <outbound />\r\n  <on-error />\r\n</policies>'
		}
	}
}

resource storageAccountConnectionStringSecret 'Microsoft.KeyVault/vaults/secrets@2019-09-01' = {
	name: '${keyVaultName}/FuncStorageConnectionString'
	dependsOn: [
		keyVault
		stg
	]
	properties: {
		value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};AccountKey=${listkeys(stg.id, '2020-08-01-preview').keys[0].value}'
	}
}

module identityApi 'api.bicep' = {
	name: 'identity.api'
	params: {
		location: location
		environmentRegionName: environmentRegionName
		keyVaultName: keyVault.name
		apimName: apim.name
		apimPricipalId: apim.identity.principalId
		apiDisplayName: 'Identity'
		apiName: 'identity'
		apiVersions: identityApiVersions
		serverFarmResourceId: serverFarm.id
		storageAccountConnectionString: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};AccountKey=${listkeys(stg.id, '2020-08-01-preview').keys[0].value}'
		appInsightsInstrumentationKey: appInsights.properties.InstrumentationKey
		apiOpenIdIssuer: apiOpenIdIssuer
		apiOpenIdClientId: apiOpenIdClientId
	}
}

module inboxApi 'api.bicep' = {
	name: 'inbox.api'
	params: {
		location: location
		environmentRegionName: environmentRegionName
		keyVaultName: keyVault.name
		apimName: apim.name
		apimPricipalId: apim.identity.principalId
		apiDisplayName: 'Inbox'
		apiName: 'inbox'
		apiVersions: inboxApiVersions
		serverFarmResourceId: serverFarm.id
		storageAccountConnectionString: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};AccountKey=${listkeys(stg.id, '2020-08-01-preview').keys[0].value}'
		appInsightsInstrumentationKey: appInsights.properties.InstrumentationKey
		apiOpenIdIssuer: apiOpenIdIssuer
		apiOpenIdClientId: apiOpenIdClientId
	}
}

module outboxApi 'api.bicep' = {
	name: 'outbox.api'
	params: {
		location: location
		environmentRegionName: environmentRegionName
		keyVaultName: keyVault.name
		apimName: apim.name
		apimPricipalId: apim.identity.principalId
		apiDisplayName: 'Outbox'
		apiName: 'outbox'
		apiVersions: outboxApiVersions
		serverFarmResourceId: serverFarm.id
		storageAccountConnectionString: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};AccountKey=${listkeys(stg.id, '2020-08-01-preview').keys[0].value}'
		appInsightsInstrumentationKey: appInsights.properties.InstrumentationKey
		apiOpenIdIssuer: apiOpenIdIssuer
		apiOpenIdClientId: apiOpenIdClientId
	}
}

module profileApi 'api.bicep' = {
	name: 'profile.api'
	params: {
		location: location
		environmentRegionName: environmentRegionName
		keyVaultName: keyVault.name
		apimName: apim.name
		apimPricipalId: apim.identity.principalId
		apiDisplayName: 'Profile'
		apiName: 'profile'
		apiVersions: profileApiVersions
		serverFarmResourceId: serverFarm.id
		storageAccountConnectionString: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};AccountKey=${listkeys(stg.id, '2020-08-01-preview').keys[0].value}'
		appInsightsInstrumentationKey: appInsights.properties.InstrumentationKey
		apiOpenIdIssuer: apiOpenIdIssuer
		apiOpenIdClientId: apiOpenIdClientId
	}
}
