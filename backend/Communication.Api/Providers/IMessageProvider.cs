namespace Communication.Api.Providers;

public interface IMessageProvider
{
    string Channel { get; }

    Task<ProviderSendResult> SendAsync(
        ProviderMessage message,
        CancellationToken cancellationToken = default);
}

public sealed record ProviderMessage(
    string Recipient,
    string Body,
    string? Subject);

public sealed record ProviderSendResult(
    bool Success,
    string? ProviderMessageId,
    string? Error);
