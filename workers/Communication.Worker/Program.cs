using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<MessageWorker>();

await builder.Build().RunAsync();

public sealed class MessageWorker(ILogger<MessageWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Communication worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            // Azure Service Bus consumption will be added here.
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
