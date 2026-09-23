# API Contract

Base URL:

`https://{api-host}/v1`

## Authentication

Machine clients send:

`Authorization: Bearer <tenant-api-key>`

Never send API keys in query strings.

## Send message

### POST /v1/messages

Example:

```json
{
  "channel": "email",
  "recipient": "customer@example.com",
  "subject": "Order update",
  "body": "Your order has shipped.",
  "idempotencyKey": "order-123-shipped"
}
```

Response:

```json
{
  "id": "message-id",
  "status": "queued"
}
```

Expected HTTP status: `202 Accepted`.

## Get message

### GET /v1/messages/{id}

Returns normalized status, recipient, channel, provider message ID and timestamps.

## List messages

### GET /v1/messages?status=delivered&limit=50

Tenant-scoped pagination.

## Webhooks

### POST /v1/webhooks/{provider}

Receives provider callbacks. Signature verification is mandatory before changing message state.

## Health

### GET /health

Returns HTTP 200 when the API process is healthy.

## Error format

```json
{
  "code": "validation_error",
  "message": "Recipient is required.",
  "traceId": "..."
}
```
