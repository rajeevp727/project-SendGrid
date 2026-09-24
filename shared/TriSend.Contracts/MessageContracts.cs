namespace TriSend.Contracts;

public enum MessageChannel
{
    Sms,
    WhatsApp,
    Email
}

public enum MessageStatus
{
    Queued,
    Processing,
    Sent,
    Delivered,
    Failed
}

public sealed record SendMessageCommand(
    Guid MessageId,
    Guid TenantId,
    MessageChannel Channel,
    string Recipient,
    string Body,
    string? Subject);
