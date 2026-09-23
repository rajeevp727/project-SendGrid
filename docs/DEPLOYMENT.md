# Deployment

## Azure resources

### Frontend
Azure Static Web Apps hosts the React dashboard.

### API
Azure App Service hosts the .NET 8 API.

### Queue
Azure Service Bus provides durable asynchronous message delivery.

### Worker
Azure Functions with Service Bus triggers is the preferred production worker host.

### Data
Azure SQL Database stores tenants, messages, events and usage.

### Secrets
Azure Key Vault stores provider API keys, webhook secrets and database credentials where needed.

### Observability
Application Insights and Azure Monitor provide logs, metrics and traces.

## GitHub secrets / configuration

Do not commit values.

Expected deployment configuration includes:

- Azure Static Web Apps deployment token or OIDC
- Azure App Service deployment identity/OIDC
- Azure resource names
- Service Bus namespace
- Key Vault name

## Deployment order

1. Create Azure resource group.
2. Create SQL database.
3. Create Service Bus namespace and queue.
4. Create Key Vault.
5. Create App Service and Function App.
6. Create Static Web App.
7. Configure application settings and managed identities.
8. Run database schema/migrations.
9. Configure provider credentials.
10. Deploy backend, worker and frontend.
11. Configure provider webhooks.
12. Execute smoke tests.
