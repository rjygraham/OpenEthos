targetScope = 'resourceGroup'

param location string
param environmentName string

resource logAnalytics 'Microsoft.OperationalInsights/workspaces@2020-10-01' = {
	name: '${environmentName}-ops-logs'
	location: location
	properties: {
		sku: {
			name: 'PerGB2018'
		}
	}
}

resource appInsights 'Microsoft.Insights/components@2020-02-02-preview' = {
	name: '${environmentName}-ai'
	location: location
	kind: 'web'
	properties: {
		Application_Type: 'web'
		WorkspaceResourceId: logAnalytics.id
	}
}

output aiInstrumentationKey string = appInsights.properties.InstrumentationKey
