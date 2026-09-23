@description('Azure region for TriSend resources.')
param location string = resourceGroup().location

@allowed(['dev', 'staging', 'prod'])
param environment string = 'dev'

param sqlAdminLogin string = 'trisendadmin'

@secure()
param sqlAdminPassword string

var suffix = '__ENV__-' + uniqueString(resourceGroup().id)
var sqlServerName = 'sql-trisend-' + suffix
var serviceBusName = 'sb-trisend-' + suffix
var apiAppName = 'app-trisend-api-' + suffix
var workerAppName = 'app-trisend-worker-' + suffix
var planName = 'asp-trisend-' + suffix
var keyVaultName = 'kv-trisend-' + uniqueString(resourceGroup().id, environment)
var appInsightsName = 'appi-trisend-' + suffix
var storageName = 'sttrisend' + uniqueString(resourceGroup().id, environment)

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
  }
}

resource storage 'Microsoft.Storage/storageAccounts@2023-05-01' = {
  name: storageName
  location: location
  sku: { name: 'Standard_LRS' }
  kind: 'StorageV2'
  properties: {
    minimumTlsVersion: 'TLS1_2'
    allowBlobPublicAccess: false
    supportsHttpsTrafficOnly: true
  }
}

resource serviceBus 'Microsoft.ServiceBus/namespaces@2022-10-01-preview' = {
  name: serviceBusName
  location: location
  sku: { name: 'Standard', tier: 'Standard' }
  properties: { publicNetworkAccess: 'Enabled' }
}

resource serviceBusQueue 'Microsoft.ServiceBus/namespaces/queues@2022-10-01-preview' = {
  parent: serviceBus
  name: 'trisend-messages'
  properties: {
    maxDeliveryCount: 10
    lockDuration: 'PT1M'
    defaultMessageTimeToLive: 'P7D'
    deadLetteringOnMessageExpiration: true
  }
}

resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' = {
  name: keyVaultName
  location: location
  properties: {
    tenantId: subscription().tenantId
    enableRbacAuthorization: true
    enableSoftDelete: true
    softDeleteRetentionInDays: 90
    publicNetworkAccess: 'Enabled'
  }
}

resource sqlServer 'Microsoft.Sql/servers@2023-08-01' = {
  name: sqlServerName
  location: location
  properties: {
    administratorLogin: sqlAdminLogin
    administratorLoginPassword: sqlAdminPassword
    minimalTlsVersion: '1.2'
    publicNetworkAccess: 'Enabled'
  }
}

resource sqlDatabase 'Microsoft.Sql/servers/databases@2023-08-01' = {
  parent: sqlServer
  name: 'TriSendDb'
  location: location
  sku: { name: 'Basic', tier: 'Basic' }
  properties: {
    maxSizeBytes: 2147483648
  }
}

resource sqlAllowAzure 'Microsoft.Sql/servers/firewallRules@2023-08-01' = {
  parent: sqlServer
  name: 'AllowAzureServices'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}

resource appServicePlan 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: planName
  location: location
  kind: 'linux'
  sku: {
    name: environment == 'prod' ? 'P1v3' : 'B1'
    tier: environment == 'prod' ? 'PremiumV3' : 'Basic'
  }
  properties: { reserved: true }
}

var commonSettings = [
  { name: 'ASPNETCORE_ENVIRONMENT', value: environment == 'prod' ? 'Production' : 'Development' }
  { name: 'APPLICATIONINSIGHTS_CONNECTION_STRING', value: appInsights.properties.ConnectionString }
  { name: 'ServiceBus__FullyQualifiedNamespace', value: serviceBus.name + '.servicebus.windows.net' }
  { name: 'ServiceBus__QueueName', value: serviceBusQueue.name }
  { name: 'KeyVault__Uri', value: keyVault.properties.vaultUri }
  { name: 'ConnectionStrings__DefaultConnection', value: 'Server=tcp:' + sqlServer.name + '.database.windows.net,1433;Initial Catalog=' + sqlDatabase.name + ';Authentication=Active Directory Default;Encrypt=True;' }
]

resource apiApp 'Microsoft.Web/sites@2023-12-01' = {
  name: apiAppName
  location: location
  kind: 'app,linux'
  identity: { type: 'SystemAssigned' }
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|8.0'
      alwaysOn: environment == 'prod'
      minTlsVersion: '1.2'
      ftpsState: 'Disabled'
      appSettings: commonSettings
    }
  }
}

resource workerApp 'Microsoft.Web/sites@2023-12-01' = {
  name: workerAppName
  location: location
  kind: 'app,linux'
  identity: { type: 'SystemAssigned' }
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|8.0'
      alwaysOn: true
      minTlsVersion: '1.2'
      ftpsState: 'Disabled'
      appSettings: commonSettings
    }
  }
}

resource apiKeyVaultRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(keyVault.id, apiApp.id, 'secrets-user')
  scope: keyVault
  properties: {
    principalId: apiApp.identity.principalId
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '4633458b-17de-408a-b874-0445c86b69e6')
    principalType: 'ServicePrincipal'
  }
}

resource workerKeyVaultRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(keyVault.id, workerApp.id, 'secrets-user')
  scope: keyVault
  properties: {
    principalId: workerApp.identity.principalId
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '4633458b-17de-408a-b874-0445c86b69e6')
    principalType: 'ServicePrincipal'
  }
}

resource apiBusRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(serviceBus.id, apiApp.id, 'sender')
  scope: serviceBus
  properties: {
    principalId: apiApp.identity.principalId
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '2b629674-e913-4c01-ae53-ef1e9d53eac2')
    principalType: 'ServicePrincipal'
  }
}

resource workerBusRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(serviceBus.id, workerApp.id, 'receiver')
  scope: serviceBus
  properties: {
    principalId: workerApp.identity.principalId
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '4a13fb42-13d1-48e4-9c98-3f7b3f7c3b7b')
    principalType: 'ServicePrincipal'
  }
}

output apiUrl string = 'https://' + apiApp.properties.defaultHostName
output workerUrl string = 'https://' + workerApp.properties.defaultHostName
output sqlServerFqdn string = sqlServer.properties.fullyQualifiedDomainName
output serviceBusNamespace string = serviceBus.name
output serviceBusQueueName string = serviceBusQueue.name
output keyVaultUri string = keyVault.properties.vaultUri
