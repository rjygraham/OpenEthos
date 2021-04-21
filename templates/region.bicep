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
param aadOpenIdConfigUrl string
param aadOpenIdApimAudience string
param aadOpenIdIssuer string
param b2cOpenIdConfigUrl string
param b2cOpenIdAudience string
param b2cOpenIdIssuer string
param aadApisClientId string
param idHintTokenSigningCertificateThumbprint string
param idHintTokenIssuer string
param idHintTokenClientId string
param o365GraphTenantId string
param o365GraphClientId string
@secure()
param o365GraphClientSecret string
param o365GraphEmailSenderObjectId string
param identityApiLoadCertificateThumbprints string

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

	resource cosmosDbAccountEndpointSecret 'secrets' = {
		name: 'CosmosDb--AccountEndpoint'
		properties: {
			value: 'https://${cosmosDb.name}.documents.azure.com:443/'
		}
	}

	resource cosmosDbAccountKeySecret 'secrets' = {
		name: 'CosmosDb--AccountKey'
		properties: {
			value: listkeys(cosmosDb.id, '2021-03-01-preview').primaryMasterKey
		}
	}

	resource cosmosDbConnectionStringSecret 'secrets' = {
		name: 'CosmosDb--ConnectionString'
		properties: {
			value: 'AccountEndpoint=https://${cosmosDb.name}.documents.azure.com:443/;AccountKey=${listkeys(cosmosDb.id, '2021-03-01-preview').primaryMasterKey};'
		}
	}

	resource idHintTokenSigningCertificateThumbprintSecret 'secrets' = {
		name: 'IdTokenHint--SigningCertificateThumbprint'
		properties: {
			value: idHintTokenSigningCertificateThumbprint
		}
	}

	resource idTokenHingIssuerSecret 'secrets' = {
		name: 'IdTokenHint--Issuer'
		properties: {
			value: idHintTokenIssuer
		}
	}

	resource idTokenHingClientIdSecret 'secrets' = {
		name: 'IdTokenHint--ClientId'
		properties: {
			value: idHintTokenClientId
		}
	}

	resource o365GraphTenantIdSecret 'secrets' = {
		name: 'O365Graph--TenantId'
		properties: {
			value: o365GraphTenantId
		}
	}

	resource o365GraphClientIdSecret 'secrets' = {
		name: 'O365Graph--ClientId'
		properties: {
			value: o365GraphClientId
		}
	}

	resource o365GraphClientSecretSecret 'secrets' = {
		name: 'O365Graph--ClientSecret'
		properties: {
			value: o365GraphClientSecret
		}
	}

	resource o365GraphEmailSenderObjectIdSecret 'secrets' = {
		name: 'O365Graph--EmailSenderObjectId'
		properties: {
			value: o365GraphEmailSenderObjectId
		}
	}

}

resource keyVaultSecretsUserRoleDefinition 'Microsoft.Authorization/roleDefinitions@2020-03-01-preview' existing = {
	name: '4633458b-17de-408a-b874-0445c86b69e6'
	scope: subscription()
}

resource msAppServiceKeyVaultRoleAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
	name: guid(keyVault.name, 'f8759b84-1481-42d2-886f-21d853735284', 'Key Vault Secrets User')
	properties: {
		principalId: 'f8759b84-1481-42d2-886f-21d853735284'
		roleDefinitionId: keyVaultSecretsUserRoleDefinition.id
	}
	scope: keyVault
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

	resource policy 'policies' = {
		name: 'policy'
		properties: {
			value: '<policies>\r\n  <inbound>\r\n    <choose>\r\n      <when condition="@(context.Request.Url.Path.Contains(&quot;/identity/&quot;))">\r\n        <validate-jwt header-name="Authorization" failed-validation-httpcode="401" failed-validation-error-message="Unauthorized" require-expiration-time="true" require-scheme="Bearer" require-signed-tokens="true" clock-skew="0">\r\n          <openid-config url="${aadOpenIdConfigUrl}" />\r\n          <audiences>\r\n            <audience>${aadOpenIdApimAudience}</audience>\r\n          </audiences>\r\n          <issuers>\r\n            <issuer>${aadOpenIdIssuer}</issuer>\r\n          </issuers>\r\n        </validate-jwt>\r\n      </when>\r\n      <otherwise>\r\n        <validate-jwt header-name="Authorization" failed-validation-httpcode="401" failed-validation-error-message="Unauthorized" require-expiration-time="true" require-scheme="Bearer" require-signed-tokens="true" clock-skew="0">\r\n          <openid-config url="${b2cOpenIdConfigUrl}" />\r\n          <audiences>\r\n            <audience>${b2cOpenIdAudience}</audience>\r\n          </audiences>\r\n          <issuers>\r\n            <issuer>${b2cOpenIdIssuer}</issuer>\r\n          </issuers>\r\n        </validate-jwt>\r\n      </otherwise>\r\n    </choose>\r\n    <authentication-managed-identity resource="${aadApisClientId}" output-token-variable-name="msi-access-token" ignore-error="false" />\r\n    <set-header name="Authorization" exists-action="override">\r\n      <value>@("Bearer " + (string)context.Variables["msi-access-token"])</value>\r\n    </set-header>\r\n  </inbound>\r\n  <backend>\r\n    <forward-request />\r\n  </backend>\r\n  <outbound />\r\n  <on-error />\r\n</policies>'
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
		aadOpenIdIssuer: aadOpenIdIssuer
		aadApisClientId: aadApisClientId
		websiteLoadCertificateThumbprints: identityApiLoadCertificateThumbprints
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
		aadOpenIdIssuer: aadOpenIdIssuer
		aadApisClientId: aadApisClientId
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
		aadOpenIdIssuer: aadOpenIdIssuer
		aadApisClientId: aadApisClientId
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
		aadOpenIdIssuer: aadOpenIdIssuer
		aadApisClientId: aadApisClientId
	}
}
