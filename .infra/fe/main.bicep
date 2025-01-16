@allowed([
  'dev'
  'prd'
])
param environment string
param location string = resourceGroup().location
param projectPrefix string = 'roosterplanner'

resource swa 'Microsoft.Web/staticSites@2024-04-01' = {
  name: '${projectPrefix}-${environment}-swa'
  location: location
  properties: {
    buildProperties: {
      skipGithubActionWorkflowGeneration: true
    }
  }
  sku: {
    tier: 'Standard'
    name: 'Standard'
  }
}
