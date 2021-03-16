targetScope = 'resourceGroup'

param name string

param location string

param identityApiVersions array = [
	'2021-01-01'
	'2021-03-15'
]

param apimPublisherEmail string = 'admin@openethos.io'
param apimPublisherName string = 'OpenEthos'

var serverFarmName = '${name}-asp'
var storageAccountName = toLower(replace('${name}funcstg', '-', ''))
var apimName = '${name}-apim'

resource serverFarm 'Microsoft.Web/serverfarms@2020-06-01' = {
	name: serverFarmName
	location: location
	kind: 'linux'
	properties: {
		reserved: true
	}
	sku: {
		tier: 'Dynamic'
		name: 'Y1'
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
}

module identityApi 'api.bicep' = {
	name: 'identity.api'
	params: {
		location: location
		name: name
		apimName: apim.name
		apiDisplayName: 'Identity'
		apiName: 'identity'
		apiVersions: identityApiVersions
		serverFarmResourceId: serverFarm.id
		storageAccountConnectionString: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};AccountKey=${listkeys(stg.id, '2020-08-01-preview').keys[0].value}'
	}
}