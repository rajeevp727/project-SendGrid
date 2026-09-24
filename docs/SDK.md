# SDK Strategy

## Canonical interface

The REST API is the canonical interface. SDKs are thin convenience wrappers.

## Initial SDKs

### .NET

Package:

`TriSend.Client`

Target: .NET 10.

Usage:

```csharp
await messaging.SendSmsAsync("+919876543210", "Your OTP is 123456");
await messaging.SendWhatsAppAsync("+919876543210", "Your order is confirmed.");
await messaging.SendEmailAsync("customer@example.com", "Order confirmed", "Your order is confirmed.");
```

### TypeScript

A lightweight client is included under `sdk/typescript`.

## Future SDKs

After the API stabilizes:

- JavaScript / npm package
- Python package
- Java package
- PHP package

Do not create SDK-specific business logic. SDKs should map cleanly to the public REST API.

## Versioning

The API is versioned using `/v1`.

Breaking API changes require a new major API version.

SDK package versions can evolve independently but must document the supported API version.
