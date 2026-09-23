using './main.bicep'

param location = 'Central India'
param environment = 'dev'
param sqlAdminLogin = 'trisendadmin'

@secure()
param sqlAdminPassword = readEnvironmentVariable('TRISEND_SQL_ADMIN_PASSWORD')
