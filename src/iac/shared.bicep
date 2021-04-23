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

  resource database 'sqlDatabases' = {
    name: 'openethos'
    properties: {
      options: {
        throughput: 400
      }
      resource: {
        id: 'openethos'
      }
    }

    resource outbox 'containers' = {
      name: 'outbox'
      properties: {
        options: {
        }
        resource: {
          id: 'outbox'
          partitionKey: {
            paths: [
              '/userId'
            ]
          }
        }
      }
    }

    resource inbox 'containers' = {
      name: 'inbox'
      properties: {
        options: {
        }
        resource: {
          id: 'inbox'
          partitionKey: {
            paths: [
              '/userId'
            ]
          }
        }
      }
    }

    resource invitations 'containers' = {
      name: 'invitations'
      properties: {
        options: {
        }
        resource: {
          id: 'invitations'
          partitionKey: {
            paths: [
              '/id'
            ]
          }
        }
      }
    }

    resource followers 'containers' = {
      name: 'followers'
      properties: {
        options: {
        }
        resource: {
          id: 'followers'
          partitionKey: {
            paths: [
              '/userId'
            ]
          }
        }
      }
    }

    resource leases 'containers' = {
      name: 'leases'
      properties: {
        options: {
        }
        resource: {
          id: 'leases'
          partitionKey: {
            paths: [
              '/id'
            ]
          }
        }
      }
    }
  }
}

output cosmosDbName string = cosmosDb.name
