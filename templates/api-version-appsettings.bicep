targetScope = 'resourceGroup'

param functionAppName string
param defaultSettings object
param existingSettings object
param constantSettings object
param newSettings object

resource appSettings 'Microsoft.Web/sites/config@2020-10-01' = {
  name: '${functionAppName}/appsettings'
  properties: union(defaultSettings, existingSettings, constantSettings, newSettings)
}
