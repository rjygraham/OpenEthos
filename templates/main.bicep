targetScope = 'subscription'

param name string
param environment string
param regions array = [
  'eastus'
]

var environmentalName = '${name}-${environment}'

var regionMap = {
  eastus: 'eus'
  westus: 'wus'
}

resource rg 'Microsoft.Resources/resourceGroups@2020-06-01' = [for region in regions: {
  name: toUpper('${environmentalName}-${regionMap[region]}')
  location: region
}]