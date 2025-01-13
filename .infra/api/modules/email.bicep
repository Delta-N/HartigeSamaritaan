@allowed([
  'dev'
  'prd'
])
param environment string
param projectPrefix string

resource emailService 'Microsoft.Communication/emailServices@2023-06-01-preview' = {
  location: 'global'
  name: '${projectPrefix}-${environment}-acs'
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
    displayName: 'Happietarria Roosterplanner'
  }
}
