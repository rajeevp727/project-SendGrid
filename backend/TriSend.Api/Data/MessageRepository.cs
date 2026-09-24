using Microsoft.Data.SqlClient;

namespace TriSend.Api.Data;

public sealed record MessageRecord(
    Guid Id,
    Guid TenantId,
    string Channel,
    string Recipient,
    string Body,
    string? Subject,
    string Status,
    string? ProviderMessageId,
    string? Error,
    DateTimeOffset CreatedAt,
    DateTimeOffset? SentAt);

public sealed class MessageRepository(IConfiguration configuration)
{
    private readonly string _connectionString =
        configuration.GetConnectionString("Sql")
        ?? throw new InvalidOperationException("ConnectionStrings:Sql is required.");

    public async Task InsertAsync(MessageRecord message, CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        const string sql = """
            INSERT INTO dbo.Messages
                (Id, TenantId, Channel, Recipient, Body, Subject, Status, CreatedAt)
            VALUES
                (@Id, @TenantId, @Channel, @Recipient, @Body, @Subject, @Status, @CreatedAt);
            """;

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", message.Id);
        command.Parameters.AddWithValue("@TenantId", message.TenantId);
        command.Parameters.AddWithValue("@Channel", message.Channel);
        command.Parameters.AddWithValue("@Recipient", message.Recipient);
        command.Parameters.AddWithValue("@Body", message.Body);
        command.Parameters.AddWithValue("@Subject", (object?)message.Subject ?? DBNull.Value);
        command.Parameters.AddWithValue("@Status", message.Status);
        command.Parameters.AddWithValue("@CreatedAt", message.CreatedAt.UtcDateTime);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<MessageRecord?> GetAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        const string sql = """
            SELECT Id, TenantId, Channel, Recipient, Body, Subject, Status,
                   ProviderMessageId, Error, CreatedAt, SentAt
            FROM dbo.Messages
            WHERE TenantId = @TenantId AND Id = @Id;
            """;

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@TenantId", tenantId);
        command.Parameters.AddWithValue("@Id", id);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
            return null;

        return new MessageRecord(
            reader.GetGuid(0), reader.GetGuid(1), reader.GetString(2),
            reader.GetString(3), reader.GetString(4),
            reader.IsDBNull(5) ? null : reader.GetString(5),
            reader.GetString(6),
            reader.IsDBNull(7) ? null : reader.GetString(7),
            reader.IsDBNull(8) ? null : reader.GetString(8),
            new DateTimeOffset(reader.GetDateTime(9), TimeSpan.Zero),
            reader.IsDBNull(10) ? null : new DateTimeOffset(reader.GetDateTime(10), TimeSpan.Zero));
    }
}
