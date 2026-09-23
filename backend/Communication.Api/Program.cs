using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Communication.Api.Data;
using Communication.Api.Messaging;
using Communication.Api.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<MessageRepository>();

var serviceBusConnectionString = builder.Configuration["ServiceBus:ConnectionString"];
var serviceBusNamespace = builder.Configuration["ServiceBus:FullyQualifiedNamespace"];

builder.Services.AddSingleton(sp =>
{
    if (!string.IsNullOrWhiteSpace(serviceBusConnectionString))
        return new ServiceBusClient(serviceBusConnectionString);

    if (string.IsNullOrWhiteSpace(serviceBusNamespace))
        throw new InvalidOperationException(
            "Configure ServiceBus:ConnectionString for local development or ServiceBus:FullyQualifiedNamespace in Azure.");

    return new ServiceBusClient(serviceBusNamespace, new DefaultAzureCredential());
});

builder.Services.AddSingleton<ServiceBusMessagePublisher>();

var app = builder.Build();

app.UseMiddleware<MvpApiKeyMiddleware>();

app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    service = "trisend-api",
    utc = DateTimeOffset.UtcNow
}));

app.MapControllers();

app.Run();
