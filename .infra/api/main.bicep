@allowed([
  'dev'
  'prd'
])
param environment string
param location string = resourceGroup().location
param projectPrefix string = 'roosterplanner'

param aspSkuName string = 'B1'
param aspSkuTier string = 'Basic'

param dbSkuName string = 'Basic'
param dbSkuTier string = 'Basic'

param authAzureTenantName string
param authB2CExtensionApplicationId string
param authClientId string
param authGraphApiScopes string
param authInstance string
param authSignUpSignInPolicyId string
param authTenantId string
param authDomain string

param sqlServerAdminGroup string

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
          name: 'KeyVaultName'
          value: kv.name
        }
        {
          name: 'AzureAuthentication__AzureTenantName'
          value: authAzureTenantName
        }
        {
          name: 'AzureAuthentication__B2CExtentionApplicationId'
          value: authB2CExtensionApplicationId
        }
        {
          name: 'AzureAuthentication__ClientId'
          value: authClientId
        }
        {
          name: 'AzureAuthentication__GraphApiScopes'
          value: authGraphApiScopes
        }
        {
          name: 'AzureAuthentication__Instance'
          value: authInstance
        }
        {
          name: 'AzureAuthentication__SignUpSignInPolicyId'
          value: authSignUpSignInPolicyId
        }
        {
          name: 'AzureAuthentication__TenantId'
          value: authTenantId
        }
        {
          name: 'AzureAuthentication__Domain'
          value: authDomain
        }
        {
          name: 'AzureAuthentication__ClientSecret'
          value: '@Microsoft.KeyVault(VaultName=${kv.name};SecretName=AzureAuthentication--ClientSecret)'
        }
        {
          name: 'ConnectionStrings__RoosterPlannerDatabase'
          value: 'Server=tcp:${sqlServer.properties.fullyQualifiedDomainName},1433;Initial Catalog=${sqldb.name};Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication="Active Directory Default";'
        }
        {
          name: 'AzureBlob__AzureBlobConnectionstring'
          value: '@Microsoft.KeyVault(SecretUri=${kv_secret_storageAccount.properties.secretUri})'
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
    administrators: {
      administratorType: 'ActiveDirectory'
      principalType: 'Group'
      sid: sqlServerAdminGroup
      tenantId: subscription().tenantId
      login: 'SqlServerAdmin'
      azureADOnlyAuthentication: true
    }
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

resource storageAccount 'Microsoft.Storage/storageAccounts@2023-05-01' = {
  name: '${projectPrefix}${environment}st'
  location: location
  kind: 'StorageV2'

  sku: {
    name: 'Standard_LRS'
  }
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    allowBlobPublicAccess: false
    supportsHttpsTrafficOnly: true
    accessTier: 'Hot'
  }
}

resource kv_secret_storageAccount 'Microsoft.KeyVault/vaults/secrets@2021-11-01-preview' = {
  name: 'AzureBlob--AzureBlobConnectionstring'
  parent: kv
  properties: {
    contentType: 'Bicep - Connection string'
    value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${az.environment().suffixes.storage};AccountKey=${storageAccount.listKeys(storageAccount.apiVersion).keys[0].value}'
  }
}
