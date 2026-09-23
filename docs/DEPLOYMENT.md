# TriSend Deployment

## Azure resources

- Azure App Service hosts the .NET 10 TriSend API.
- Azure Functions isolated .NET 10 hosts the Service Bus worker.
- Azure Service Bus provides durable asynchronous message delivery.
- Azure SQL Database stores tenants and messages.
- Azure Key Vault stores provider secrets and application secrets.
- Application Insights provides telemetry.

## Required API settings

- `Mvp__ApiKey`
- `Mvp__TenantId`
- `ConnectionStrings__Sql`
- `ServiceBus__FullyQualifiedNamespace`
- `ServiceBus__QueueName`

## Required Function settings

- `FUNCTIONS_WORKER_RUNTIME=dotnet-isolated`
- `ServiceBusQueueName=trisend-messages`
- `ServiceBusConnection__fullyQualifiedNamespace=<namespace>.servicebus.windows.net`
- `ConnectionStrings__Sql=<SQL connection string using Entra authentication>`

The Function's managed identity needs Service Bus Data Receiver and SQL database permissions.

The API's managed identity needs Service Bus Data Sender and SQL database permissions.

## Deployment order

1. Create Azure resources.
2. Create the SQL schema from `infrastructure/sql/001_initial_schema.sql`.
3. Configure managed identities/RBAC.
4. Configure Key Vault and application settings.
5. Deploy the API.
6. Deploy the Function worker.
7. Run the smoke test.

See [Smoke Test](SMOKE-TEST.md) for the end-to-end verification.
