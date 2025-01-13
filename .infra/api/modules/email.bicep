@allowed([
  'dev'
  'prd'
])
param environment string
param projectPrefix string

resource emailService 'Microsoft.Communication/emailServices@2023-06-01-preview' = {
  location: 'global'
  name: '${projectPrefix}-${environment}-acse'
  properties: {
    dataLocation: 'europe'
  }
}

resource acsAzManagedDomain 'Microsoft.Communication/emailServices/domains@2023-06-01-preview' = {
  parent: emailService
  name: '${projectPrefix}-${environment}-acse-domain'
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
