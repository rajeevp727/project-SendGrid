using Microsoft.AspNetCore.Mvc;

namespace Communication.Api.Controllers;

[ApiController]
[Route("v1/messages")]
public class MessagesController : ControllerBase
{
    [HttpPost]
    public IActionResult Send([FromBody] SendMessageRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Recipient) ||
            string.IsNullOrWhiteSpace(request.Body))
        {
            return BadRequest("Recipient and body are required.");
        }

        var id = Guid.NewGuid();

        return Accepted(new
        {
            id,
            status = "queued",
            channel = request.Channel
        });
    }
}

public sealed record SendMessageRequest(
    string Channel,
    string Recipient,
    string Body,
    string? Subject = null);
