# TriSend

An API-first communications platform for **SMS, WhatsApp and email**.

Other applications integrate with TriSend once instead of integrating separately with messaging vendors.

## MVP flow

```
Your App -> TriSend API -> Azure SQL -> Azure Service Bus -> Azure Function -> Provider -> Status
```

### Send a message

```http
POST /v1/messages
Authorization: Bearer <API_KEY>
Content-Type: application/json
```

```json
{
  "channel": "sms",
  "recipient": "+919876543210",
  "body": "Your OTP is 123456"
}
```

The API returns HTTP 202 with a queued message ID. The Function worker consumes the queue and updates the message status.

## Supported channels

- SMS
- WhatsApp
- Email

The MVP ships with a development provider so the complete queue/worker/status path can be verified before adding production provider credentials.

## Documentation

- [API](docs/API.md)
- [MVP](docs/MVP.md)
- [Architecture](docs/architecture.md)
- [Deployment](docs/DEPLOYMENT.md)
- [Local development](docs/LOCAL-DEVELOPMENT.md)
- [Smoke test](docs/SMOKE-TEST.md)
