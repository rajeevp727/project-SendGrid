# Consuming Project SendGrid

Project SendGrid is an API-first communications platform.

Other applications should NOT integrate directly with Twilio, WhatsApp Cloud API, SMTP or individual SMS vendors. They integrate with Project SendGrid once, and Project SendGrid handles provider selection, authentication, retries, delivery status and webhooks.

## Integration model

```text
GreenPantry / SprintDeck / Any App
             |
             | HTTPS + API Key
             v
      Project SendGrid API
             |
       Azure Service Bus
             |
          Worker
      /       |       \
    SMS    WhatsApp   Email
    Provider Provider Provider
```

## REST API

Every application can consume the platform without an SDK.

Base URL:

`https://api.your-domain.com/v1`

Authentication:

`Authorization: Bearer <API_KEY>`

### Send SMS

```http
POST /v1/messages
Authorization: Bearer <API_KEY>
Content-Type: application/json

{
  "channel": "sms",
  "recipient": "+919876543210",
  "body": "Your OTP is 123456",
  "idempotencyKey": "otp-login-123456"
}
```

### Send WhatsApp

```http
POST /v1/messages
Authorization: Bearer <API_KEY>
Content-Type: application/json

{
  "channel": "whatsapp",
  "recipient": "+919876543210",
  "body": "Your order #10045 has been confirmed.",
  "idempotencyKey": "order-10045-confirmed"
}
```

### Send Email

```http
POST /v1/messages
Authorization: Bearer <API_KEY>
Content-Type: application/json

{
  "channel": "email",
  "recipient": "customer@example.com",
  "subject": "Order confirmed",
  "body": "Your order #10045 has been confirmed.",
  "idempotencyKey": "order-10045-email"
}
```

## SDK model

For .NET applications, expose a typed client:

```csharp
services.AddProjectSendGrid(options =>
{
    options.BaseUrl = "https://api.your-domain.com";
    options.ApiKey = configuration["ProjectSendGrid:ApiKey"]!;
});

await client.SendSmsAsync("+919876543210", "Your OTP is 123456");
await client.SendWhatsAppAsync("+919876543210", "Your order is confirmed.");
await client.SendEmailAsync(
    "customer@example.com",
    "Order confirmed",
    "Your order is confirmed.");
```

The SDK is a convenience layer. The REST API remains the canonical contract.

## API key isolation

Create a separate API key per consuming application/environment where practical:

- GreenPantry-dev
- GreenPantry-prod
- SprintDeck-dev
- SprintDeck-prod

This makes rotation, revocation and usage tracking possible without changing application code.

## Important design rule

Consumer applications know only:

- Project SendGrid API URL
- API key
- channel
- recipient
- message/template data

Consumer applications do NOT know provider credentials.
