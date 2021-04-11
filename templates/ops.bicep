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

output logAnalyticsName string = logAnalytics.name
