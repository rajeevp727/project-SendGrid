using System.Text.Json;
using Communication.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Communication.Worker;

public sealed class MessageProcessor(
    IConfiguration configuration,
    ILogger<MessageProcessor> logger)
{
    [Function("ProcessMessage")]
    public async Task Run(
        [ServiceBusTrigger("%ServiceBusQueueName%", Connection = "ServiceBusConnection")]
        string body,
        CancellationToken cancellationToken)
    {
        var command = JsonSerializer.Deserialize<SendMessageCommand>(body)
            ?? throw new InvalidOperationException("Invalid TriSend message payload.");

        await UpdateStatusAsync(command.MessageId, "processing", null, cancellationToken);

        // Development provider: this proves the full queue -> worker -> persistence path.
        // Real SMS/WhatsApp/email adapters plug into the same provider boundary.
        var providerMessageId = $"dev_{Guid.NewGuid():N}";

        logger.LogInformation(
            "TriSend worker processed message {MessageId}. Channel={Channel} ProviderMessageId={ProviderMessageId}",
            command.MessageId, command.Channel, providerMessageId);

        await UpdateStatusAsync(command.MessageId, "sent", providerMessageId, cancellationToken);
    }

    private async Task UpdateStatusAsync(
        Guid messageId,
        string status,
        string? providerMessageId,
        CancellationToken cancellationToken)
    {
        var connectionString = configuration.GetConnectionString("Sql")
            ?? throw new InvalidOperationException("ConnectionStrings:Sql is required.");

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        const string sql = """
            UPDATE dbo.Messages
            SET Status = @Status,
                ProviderMessageId = COALESCE(@ProviderMessageId, ProviderMessageId),
                SentAt = CASE WHEN @Status = 'sent' THEN SYSUTCDATETIME() ELSE SentAt END
            WHERE Id = @Id;
            """;

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", messageId);
        command.Parameters.AddWithValue("@Status", status);
        command.Parameters.AddWithValue("@ProviderMessageId", (object?)providerMessageId ?? DBNull.Value);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}
