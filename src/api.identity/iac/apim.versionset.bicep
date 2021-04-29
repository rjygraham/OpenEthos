targetScope = 'resourceGroup'

param apimName string
param apiName string
param apiDisplayName string

resource apim 'Microsoft.ApiManagement/service@2021-01-01-preview' existing = {
  name: apimName
}

resource apiVersionSet 'Microsoft.ApiManagement/service/api-version-sets@2018-06-01-preview' = {
	name: '${apim.name}/${apiName}'
	properties: {
		displayName: '${apiDisplayName}'
		versioningScheme: 'Header'
		versionHeaderName: 'api-version'
	}
}
