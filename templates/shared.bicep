targetScope = 'resourceGroup'

param location string
param environmentName string

var cosmosName = '${environmentName}-cosmos'

resource cosmosDb 'Microsoft.DocumentDB/databaseAccounts@2020-06-01-preview' = {
  name: cosmosName
  location: location
  properties: {
    createMode: 'Default'
    enableFreeTier: true
    databaseAccountOfferType: 'Standard'
    locations: [
      {
        locationName: 'eastus'
      }
    ]
  }
}

output cosmosDbName string = cosmosDb.name
