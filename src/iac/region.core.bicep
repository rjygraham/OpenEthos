targetScope = 'resourceGroup'

param location string
param environmentRegionName string
param opsResourceGroupName string
param sharedResourceGoupName string
param cosmosDbName string
param apimPublisherEmail string
param apimPublisherName string
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

var serverFarmName = '${environmentRegionName}-asp'
var apimName = '${environmentRegionName}-apim'
var keyVaultName = '${environmentRegionName}-kvlt'

resource cosmosDb 'Microsoft.DocumentDB/databaseAccounts@2021-03-01-preview' existing = {
	name: cosmosDbName
	scope: resourceGroup(sharedResourceGoupName)
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

// This grants App Service/Functions ability to read AppSettings as Key Vault references for RBAC enabled Key Vaults. 
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
