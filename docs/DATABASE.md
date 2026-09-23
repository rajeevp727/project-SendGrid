# Database Design

## Tenant-owned entities

```text
Tenant
 ├── ApiKey
 ├── ProviderAccount
 ├── Message
 │    ├── MessageAttempt
 │    └── MessageEvent
 ├── Template
 └── Usage
```

## Rules

- Every tenant-owned table includes TenantId.
- Never trust TenantId from a client request.
- TenantId comes from authenticated credentials.
- Messages are immutable in their core identity fields.
- Status transitions are recorded as events.
- Provider payloads are retained only as required for debugging/compliance.

## Required indexes

- Messages(TenantId, CreatedAtUtc)
- Messages(TenantId, Status, CreatedAtUtc)
- MessageEvents(MessageId, CreatedAtUtc)
- ApiKeys(KeyHash)
