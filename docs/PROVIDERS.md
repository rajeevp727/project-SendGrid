# Provider Integration

## Adapter contract

All channel integrations implement a common application contract.

```csharp
public interface IMessageProvider
{
    MessageChannel Channel { get; }
    Task<ProviderSendResult> SendAsync(
        Message message,
        CancellationToken cancellationToken);
}
```

The application must never call vendor SDKs directly from controllers.

## Provider responsibilities

- Validate provider configuration.
- Map platform message to vendor request.
- Send the message.
- Normalize vendor response.
- Verify webhook signatures.
- Map vendor status to platform status.

## Initial provider strategy

Use one provider per channel for the first production MVP. Keep the adapter boundary so a second provider can be added without changing the public API.

Provider credentials are loaded from Azure Key Vault or managed configuration. No credentials belong in Git.
