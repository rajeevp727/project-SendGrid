using System.Net.Http.Json;

namespace TriSend.Client;

public sealed class TriSendClient(HttpClient httpClient)
{
    public async Task<SendMessageResponse> SendSmsAsync(
        string recipient,
        string body,
        string? idempotencyKey = null,
        CancellationToken cancellationToken = default) =>
        await SendAsync(new SendMessageRequest(
            "sms", recipient, body, null, idempotencyKey), cancellationToken);

    public async Task<SendMessageResponse> SendWhatsAppAsync(
        string recipient,
        string body,
        string? idempotencyKey = null,
        CancellationToken cancellationToken = default) =>
        await SendAsync(new SendMessageRequest(
            "whatsapp", recipient, body, null, idempotencyKey), cancellationToken);

    public async Task<SendMessageResponse> SendEmailAsync(
        string recipient,
        string subject,
        string body,
        string? idempotencyKey = null,
        CancellationToken cancellationToken = default) =>
        await SendAsync(new SendMessageRequest(
            "email", recipient, body, subject, idempotencyKey), cancellationToken);

    private async Task<SendMessageResponse> SendAsync(
        SendMessageRequest request,
        CancellationToken cancellationToken)
    {
        using var response = await httpClient.PostAsJsonAsync(
            "v1/messages", request, cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<SendMessageResponse>(
            cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("TriSend returned an empty response.");
    }
}

public sealed record SendMessageRequest(
    string Channel,
    string Recipient,
    string Body,
    string? Subject,
    string? IdempotencyKey);

public sealed record SendMessageResponse(
    Guid Id,
    string Status);
