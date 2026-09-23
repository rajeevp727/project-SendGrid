namespace Communication.Api.Providers;

public sealed class DevelopmentMessageProvider(ILogger<DevelopmentMessageProvider> logger)
    : IMessageProvider
{
    public string Channel => "development";

    public Task<ProviderSendResult> SendAsync(
        ProviderMessage message,
        CancellationToken cancellationToken = default)
    {
        var providerMessageId = $"dev_{Guid.NewGuid():N}";

        logger.LogInformation(
            "Development message accepted. ProviderMessageId={ProviderMessageId} Recipient={Recipient}",
            providerMessageId,
            message.Recipient);

        return Task.FromResult(
            new ProviderSendResult(true, providerMessageId, null));
    }
}
