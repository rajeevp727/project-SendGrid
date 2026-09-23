# Project SendGrid

An API-first communications platform for **SMS, WhatsApp and email**.

Other applications integrate with Project SendGrid once instead of integrating separately with messaging vendors.

## Example

```csharp
await messaging.SendSmsAsync("+919876543210", "Your OTP is 123456");
await messaging.SendWhatsAppAsync("+919876543210", "Your order is confirmed.");
await messaging.SendEmailAsync("customer@example.com", "Order confirmed", "Your order is confirmed.");
```

Any language can consume the REST API with:

```http
POST /v1/messages
Authorization: Bearer <API_KEY>
Content-Type: application/json
```

## MVP architecture

```text
Your App -> Project SendGrid API -> Service Bus -> Worker -> SMS / WhatsApp / Email provider
```

## Documentation

- [Consumption guide](docs/CONSUMPTION.md)
- [MVP](docs/MVP.md)
- [API](docs/API.md)
- [SDK strategy](docs/SDK.md)
- [Architecture](docs/architecture.md)
- [Deployment](docs/DEPLOYMENT.md)
- [Local development](docs/LOCAL-DEVELOPMENT.md)
- [Smoke test](docs/SMOKE-TEST.md)
