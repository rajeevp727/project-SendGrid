# TriSend.Client

Typed .NET 10 client for consuming TriSend.

## Register

```csharp
builder.Services.AddTriSend(options =>
{
    options.BaseUrl = configuration["TriSend:BaseUrl"]!;
    options.ApiKey = configuration["TriSend:ApiKey"]!;
});
```

## Inject

```csharp
public class OrderService(TriSendClient messaging)
{
    public Task SendOrderEmailAsync()
    {
        return messaging.SendEmailAsync(
            "customer@example.com",
            "Order confirmed",
            "Your order has been confirmed.");
    }
}
```

The same client exposes SendSmsAsync and SendWhatsAppAsync.
