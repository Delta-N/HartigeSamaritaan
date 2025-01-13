@allowed([
  'dev'
  'prd'
])
param environment string
param projectPrefix string

@description('Name of the keyvault resource that the connection string will be added to')
param kvName string

resource emailService 'Microsoft.Communication/emailServices@2023-06-01-preview' = {
  location: 'global'
  name: '${projectPrefix}-${environment}-acse'
  properties: {
    dataLocation: 'europe'
  }
}

resource acsAzManagedDomain 'Microsoft.Communication/emailServices/domains@2023-06-01-preview' = {
  parent: emailService
  name: 'AzureManagedDomain'
  location: 'Global'
  properties: {
    domainManagement: 'AzureManaged'
    userEngagementTracking: 'Disabled'
  }
}

resource acsAzManagedDomainTestUser 'Microsoft.Communication/emailServices/domains/senderUsernames@2023-06-01-preview' = {
  parent: acsAzManagedDomain
  name: 'happietariaroosterplanner'
  properties: {
    username: 'happietariaroosterplanner'
    displayName: 'Happietaria Roosterplanner'
  }
}

resource acsCommunicationService 'Microsoft.Communication/communicationServices@2023-06-01-preview' = {
  name: '${projectPrefix}-${environment}-acs'
  location: 'Global'
  properties: {
    dataLocation: 'europe'
    linkedDomains: [
      acsAzManagedDomain.id
    ]
  }
}

resource kv 'Microsoft.KeyVault/vaults@2022-07-01' existing = {
  name: kvName
}

resource kv_secret_emailConnectionString 'Microsoft.KeyVault/vaults/secrets@2021-11-01-preview' = {
  name: 'ACSConfig--ConnectionString'
  parent: kv
  properties: {
    contentType: 'Bicep - Connection string'
    value: acsCommunicationService.listKeys(acsCommunicationService.apiVersion).keys[0].value
  }
}

output emailConnectionStringUri string = kv_secret_emailConnectionString.properties.secretUri
output senderEmail string = acsAzManagedDomainTestUser.properties.username
