using System.Text.Json;
using Azure.Messaging.ServiceBus;
using TriSend.Contracts;

namespace TriSend.Api.Messaging;

public sealed class ServiceBusMessagePublisher(
    ServiceBusClient client,
    IConfiguration configuration) : IAsyncDisposable
{
    private readonly ServiceBusSender _sender =
        client.CreateSender(configuration["ServiceBus:QueueName"] ?? "trisend-messages");

    public async Task PublishAsync(SendMessageCommand command, CancellationToken cancellationToken)
    {
        var body = JsonSerializer.Serialize(command);
        await _sender.SendMessageAsync(
            new ServiceBusMessage(body)
            {
                ContentType = "application/json",
                Subject = command.Channel.ToString().ToLowerInvariant(),
                MessageId = command.MessageId.ToString()
            },
            cancellationToken);
    }

    public ValueTask DisposeAsync() => _sender.DisposeAsync();
}
