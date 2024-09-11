using Messaging.Data;
using Messaging.Hubs;
using Messaging.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Messaging.Areas.Messaging.Controllers;

/// <summary>
/// API controller for handling chat message operations.
/// It allows sending messages and reading the latest messages from the past 10 minutes.
/// </summary>
/// <param name="applicationDatabaseContext">Database context for handling message data.</param>
/// <param name="hubContext">SignalR Hub context for broadcasting real-time messages.</param>
/// <param name="logger">Instance of the logger for tracking controller activities.</param>
[ApiController]
[Area(areaName: "Messaging")]
[Route(template: "messaging/chat/api")]
public class ChatApiController(
    ApplicationDatabaseContext applicationDatabaseContext,
    IHubContext<ChatHub> hubContext,
    ILogger<ChatApiController> logger
) : Controller
{
    private readonly ApplicationDatabaseContext _applicationDatabaseContext =
        applicationDatabaseContext;
    private readonly IHubContext<ChatHub> _hubContext = hubContext;
    private readonly ILogger<ChatApiController> _logger = logger;

    /// <summary>
    /// Sends a message to the chat.
    /// The message is saved to the database and broadcast to all connected clients via SignalR.
    /// </summary>
    /// <param name="message">Message object containing the sender's name and message text.</param>
    /// <returns>Returns a success response if the message is sent, or an error if the message is empty.</returns>
    [HttpPost(template: "send")]
    public async Task<IActionResult> Send([FromForm] Message message)
    {
        _logger.LogInformation(message: "Messaging ChatApi Send interact");
        if (string.IsNullOrEmpty(value: message.Name) || string.IsNullOrEmpty(value: message.Text))
        {
            _logger.LogWarning(message: "Empty message received.");
            return BadRequest(error: "Message cannot be empty.");
        }
        message.Timestamp = DateTime.UtcNow;
        await _applicationDatabaseContext.AddAsync(entity: message);
        await _applicationDatabaseContext.SaveChangesAsync();
        await _hubContext.Clients.All.SendAsync(method: "ReceiveMessage", arg1: message);
        _logger.LogInformation(
            message: "From |{Name}|\nMessage: |{Text}|.",
            args: [message.Name, message.Text]
        );
        return Ok(value: "Message sent successfully.");
    }

    /// <summary>
    /// Retrieves the latest messages sent within the last 10 minutes.
    /// </summary>
    /// <returns>A list of messages sent within the last 10 minutes, or a message indicating no messages were found.</returns>
    [HttpGet(template: "read-latest")]
    public IActionResult ReadLatest()
    {
        _logger.LogInformation(message: "Messaging ChatApi ReadLatest interact");
        DateTime tenMinutesAgo = DateTime.UtcNow.AddMinutes(-10);
        IOrderedQueryable<Message> latestMessages =
            from message in _applicationDatabaseContext.Messages
            where message.Timestamp >= tenMinutesAgo
            orderby message.Timestamp descending
            select message;
        if (!latestMessages.Any())
        {
            _logger.LogInformation(message: "No messages found in the last 10 minutes.");
            return Ok(value: "No messages found in the last 10 minutes.");
        }
        _logger.LogInformation(
            message: "Latest messages read. Count: |{Count}|.",
            latestMessages.Count()
        );
        return Ok(value: latestMessages);
    }
}
