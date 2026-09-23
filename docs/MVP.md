# MVP Definition

## Product goal

Project SendGrid is a developer-first messaging platform that lets a business create a tenant, obtain an API key, send a message through one API, and receive delivery status through webhooks.

## MVP channels

1. Email
2. SMS
3. WhatsApp

The platform owns the API, tenant model, message lifecycle, usage tracking and provider abstraction. It does not operate telecom infrastructure.

## MVP user journey

1. Sign up / create tenant.
2. Configure a provider.
3. Generate an API key.
4. Send a message using POST /v1/messages.
5. API validates and queues the message.
6. Worker sends through the selected provider.
7. Provider webhook updates status.
8. Dashboard shows message and delivery history.
9. Usage page shows message counts.

## Must-have

- Tenant isolation
- API key authentication
- Send message API
- Email provider integration
- SMS provider integration
- WhatsApp provider integration
- Provider abstraction
- Azure Service Bus queue
- Retry for transient provider failures
- Message status lifecycle
- Provider webhooks
- Azure SQL persistence
- Basic React dashboard
- Usage counters
- Structured logging
- Health endpoint
- GitHub Actions CI/CD
- Secrets in Azure Key Vault

## Explicitly out of MVP

- Marketing campaign builder
- Contact segmentation
- Visual workflow automation
- Advanced billing/invoicing
- Dedicated phone-number marketplace
- AI campaign generation
- Multi-region active-active deployment
- Complex RBAC
- Customer-facing SDKs for every language

## Definition of done

A developer can configure one provider per supported channel, call the API with a valid tenant API key, observe the message move from queued to provider status, and see the result in the dashboard without manually editing database records.
