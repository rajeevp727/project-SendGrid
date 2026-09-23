# Local Development

## Prerequisites

- .NET 8 SDK
- Node.js 22+
- Git
- Azure CLI for Azure-connected development

## Backend

```bash
dotnet run --project backend/Communication.Api/Communication.Api.csproj
```

Health check:

```text
GET http://localhost:5000/health
```

## Worker

```bash
dotnet run --project workers/Communication.Worker/Communication.Worker.csproj
```

The worker currently contains the host skeleton. Azure Service Bus consumption is added during the messaging implementation phase.

## Frontend

```bash
cd frontend
npm install
npm run dev
```

Set the API URL through the Vite environment configuration.

## Local secrets

Use user-secrets or local environment variables. Never commit provider credentials.
