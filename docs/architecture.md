# Architecture

```mermaid
graph TD
    C[Customer / Dashboard] --> SWA[Azure Static Web Apps - React]
    C --> API[Azure App Service - .NET 8 API]
    API --> SQL[Azure SQL Database]
    API --> SB[Azure Service Bus]
    API --> KV[Azure Key Vault]
    SB --> W[Azure Functions / Worker]
    W --> SMS[SMS Provider]
    W --> WA[WhatsApp Provider]
    W --> EM[Email Provider]
    SMS --> WH[Provider Webhooks]
    WA --> WH
    EM --> WH
    WH --> API
    API --> AI[Application Insights]
    W --> AI
```

## Message flow

1. API authenticates the tenant and validates the request.
2. Message is persisted as queued.
3. API publishes a command to Service Bus.
4. Worker selects the channel provider and sends the message.
5. Provider response is persisted.
6. Provider webhook updates delivery status.
7. Dashboard reads the resulting status and usage.

Provider credentials belong in Azure Key Vault. Never commit credentials.
