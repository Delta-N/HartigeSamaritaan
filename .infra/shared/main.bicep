param environment string
param projectPrefix string = 'roosterplanner'
param location string = resourceGroup().location
param tenantId string = subscription().tenantId

resource kv 'Microsoft.KeyVault/vaults@2022-07-01' = {
  name: '${projectPrefix}-${environment}-kv'
  location: location
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: tenantId
    accessPolicies: []
  }
}
