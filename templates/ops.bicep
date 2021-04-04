targetScope = 'resourceGroup'

param location string
param logAnalyticsName string

resource logAnalytics 'Microsoft.OperationalInsights/workspaces@2020-10-01' = {
	name: logAnalyticsName
	location: location
	properties: {
		sku: {
			name: 'PerGB2018'
		}
	}
}