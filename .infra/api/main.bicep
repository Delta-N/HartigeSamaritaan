@allowed([
  'dev'
  'prd'
])
param environment string
param location string = resourceGroup().location
param projectPrefix string = 'roosterplanner'
param aspSkuName string = 'B1'
param aspSkuTier string = 'Basic'

param dbSkuName string = 'GP_Gen5_2'
param dbSkuTier string = 'GeneralPurpose'

var tenantId = subscription().tenantId

resource kv 'Microsoft.KeyVault/vaults@2022-07-01' = {
  name: '${projectPrefix}-${environment}-kv'
  location: location
}

resource asp 'Microsoft.Web/serverfarms@2024-04-01' = {
  name: '${projectPrefix}-${environment}-asp'
  location: location
  kind: 'linux'
  sku: {
    name: aspSkuName
    tier: aspSkuTier
  }
  properties: { reserved: true }
}

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2021-06-01' = {
  name: '${projectPrefix}-${environment}-law'
  location: location
  properties: {
    features: {}
    publicNetworkAccessForIngestion: 'Enabled'
    retentionInDays: 30
    sku: {
      name: 'PerGB2018'
    }
  }
}

resource appi 'Microsoft.Insights/components@2020-02-02' = {
  name: '${projectPrefix}-${environment}-appi'
  kind: 'web'
  location: location
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logAnalyticsWorkspace.id
  }
}

resource webapi 'Microsoft.Web/sites@2024-04-01' = {
  location: location
  name: '${projectPrefix}-${environment}-api'
  kind: 'app'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: asp.id
    siteConfig: {
      alwaysOn: true
      linuxFxVersion: 'DOTNETCORE|8.0'
      appSettings: [
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: 'Production'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appi.properties.InstrumentationKey
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appi.properties.ConnectionString
        }
        {
          name: 'ApplicationInsightsAgent_EXTENSION_VERSION'
          value: '~3'
        }
        {
          name: 'XDT_MicrosoftApplicationInsights_Mode'
          value: 'recommended'
        }
        {
          name: 'XDT_MicrosoftApplicationInsights_PreemptSdk'
          value: '1'
        }
        {
          name: 'KeyVault__Uri'
          value: kv.properties.vaultUri
        }
      ]
    }
  }
}

resource kvAccessPolicy 'Microsoft.KeyVault/vaults/accessPolicies@2021-10-01' = {
  name: 'add'
  parent: kv
  properties: {
    accessPolicies: [
      {
        tenantId: tenantId
        objectId: webapi.identity.principalId
        permissions: {
          keys: [
            'get'
            'list'
          ]
          secrets: [
            'get'
            'list'
          ]
        }
      }
    ]
  }
}

resource sqlServer 'Microsoft.Sql/servers@2024-05-01-preview' = {
  location: location
  name: '${projectPrefix}-${environment}-sql'
  properties: {
    publicNetworkAccess: 'Enabled'
  }
}

resource sqldb 'Microsoft.Sql/servers/databases@2021-11-01' = {
  name: '${projectPrefix}-${environment}-sqldb'
  parent: sqlServer
  location: location
  sku: {
    name: dbSkuName
    tier: dbSkuTier
  }
  properties: {
    zoneRedundant: false
  }
}

resource keyVaultDevHub_Secrets_DbCon 'Microsoft.KeyVault/vaults/secrets@2021-11-01-preview' = {
  name: 'ConnectionStrings--HubDbContext'
  parent: keyVaultDevHub
  properties: {
    contentType: 'ARM - ConnectionString'
    value: 'Server=tcp:${sqlServerDevHub.properties.fullyQualifiedDomainName},1433;Initial Catalog=${sqlDbDevHub.name};Persist Security Info=False;User ID=${sqlDevHub_Settings.serverLoginName};Password=${sqlDevHub_Settings.serverLoginPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
  }
}
