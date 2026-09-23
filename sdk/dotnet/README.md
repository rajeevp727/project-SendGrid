# ProjectSendGrid.Client

Typed .NET 8 client for consuming Project SendGrid.

## Register

```csharp
builder.Services.AddProjectSendGrid(options =>
{
    options.BaseUrl = configuration["ProjectSendGrid:BaseUrl"]!;
    options.ApiKey = configuration["ProjectSendGrid:ApiKey"]!;
});
```

## Inject

```csharp
public class OrderService(ProjectSendGridClient messaging)
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
