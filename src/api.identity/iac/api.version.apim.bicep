targetScope = 'resourceGroup'

param apimName string
param funcName string
param funcId string
param funcHostName string
param apiName string
param apiDisplayName string
param version string
@secure()
param funcHostKey string

resource apimFunctionKeyNamedValue 'Microsoft.ApiManagement/service/namedValues@2020-06-01-preview' = {
	name: '${apimName}/${funcName}-key'
	properties: {
		displayName: '${funcName}-key'
		secret: true
		value: funcHostKey
		tags: [
			'key'
			'function'
		]
	}
}

resource apimBackend 'Microsoft.ApiManagement/service/backends@2020-06-01-preview' = {
	name: '${apimName}/${apiName}-${version}'
	dependsOn: [
		apimFunctionKeyNamedValue
	]
	properties: {
		protocol: 'http'
		url: 'https://${funcHostName}/api'
		resourceId: 'https://management.azure.com${funcId}'
		credentials: {
			header: {
				'x-functions-key': [
					'{{${funcName}-key}}'
				]
			}
		}
	}
}

resource api 'Microsoft.ApiManagement/service/apis@2020-06-01-preview' = {
	name: '${apimName}/${apiName}-${version}'
	properties: {
		apiType: 'http'
		apiVersion: version
		apiVersionSetId: '/apiVersionSets/${apiName}'
		path: apiName
		displayName: apiDisplayName
		protocols: [
			'https'
		]
	}

	resource policy 'policies@2020-12-01' = {
		name: 'policy'
		dependsOn: [
			apimBackend
		]
		properties: {
			value: '<policies>\r\n  <inbound>\r\n    <base />\r\n    <set-backend-service backend-id="${apiName}-${version}" />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>'
		}
	}
}
