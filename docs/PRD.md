# Product Requirements Document

## 1. Problem

Businesses integrate multiple communication providers separately. Project SendGrid provides one API and dashboard for transactional SMS, WhatsApp and email.

## 2. Target users

- SaaS developers
- Small and medium businesses
- Internal engineering teams
- Product teams sending transactional notifications

## 3. Core capabilities

### Tenants
A tenant represents one customer account and owns its API keys, provider accounts, messages and usage.

### API keys
Machine clients authenticate using a tenant-scoped secret. Keys are stored hashed and displayed only once.

### Messages
A message contains channel, recipient, content, status, timestamps and provider metadata.

### Providers
Provider adapters hide vendor-specific APIs behind a common interface.

### Webhooks
Provider callbacks are verified, normalized and stored as message events.

### Usage
Each accepted message increments usage counters by tenant and channel.

## 4. Message states

```text
Queued -> Processing -> Sent -> Delivered
                     \-> Failed
```

Provider-specific states are normalized into the platform states.

## 5. Reliability requirements

- Idempotency key supported on send requests.
- Retry transient provider failures with exponential backoff.
- Dead-letter messages after retry exhaustion.
- Never log provider secrets or message credentials.
- Webhook processing must be idempotent.

## 6. Security requirements

- HTTPS only.
- Tenant isolation at service/repository level.
- API keys hashed at rest.
- Provider credentials stored in Key Vault.
- Request validation and rate limiting.
- Webhook signature verification.
- Audit events for security-sensitive changes.

## 7. Non-functional targets for MVP

- API health endpoint available.
- Queue-based delivery so provider latency does not block the API.
- Structured logs with correlation/message IDs.
- Database indexes for tenant + message time queries.
- Automated build on every pull request.
