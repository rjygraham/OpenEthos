targetScope = 'resourceGroup'

param keyVaultName string
param apiFuncName string
param apiFuncPrincipalId string

resource keyVaultSecretsUserRoleDefinition 'Microsoft.Authorization/roleDefinitions@2020-03-01-preview' existing = {
	name: '4633458b-17de-408a-b874-0445c86b69e6'
	scope: subscription()
}

resource keyVault 'Microsoft.KeyVault/vaults@2020-04-01-preview' existing = {
	name: keyVaultName
	scope: resourceGroup()
}

resource funcAppKeyVaultRoleAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
	name: guid(apiFuncName, keyVault.name, 'Key Vault Secrets User')
	properties: {
		principalId: apiFuncPrincipalId
		roleDefinitionId: keyVaultSecretsUserRoleDefinition.id
	}
	scope: keyVault
}
