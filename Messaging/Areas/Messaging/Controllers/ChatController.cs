using Microsoft.AspNetCore.Mvc;

namespace Messaging.Areas.Messaging.Controllers;

/// <summary>
/// Controller for managing chat pages within the "Messaging" area.
/// Provides methods for sending and reading messages.
/// </summary>
/// <param name="logger">An instance of the logger for tracking controller activities.</param>
[Area(areaName: "Messaging")]
[Controller]
[Route(template: "messaging/chat")]
public class ChatController(ILogger<ChatController> logger) : Controller
{
    private readonly ILogger<ChatController> _logger = logger;

    /// <summary>
    /// Displays the main chat page.
    /// </summary>
    /// <returns>Returns the view for the chat's main page.</returns>
    [HttpGet(template: "/")]
    [HttpGet(template: "/messaging")]
    [HttpGet(template: "")]
    public IActionResult Index()
    {
        _logger.LogInformation(message: "Messaging Chat Index interact");
        return View();
    }

    /// <summary>
    /// Displays the page for sending messages.
    /// </summary>
    /// <returns>Returns the view for the message sending page.</returns>
    [HttpGet(template: "send")]
    public IActionResult Send()
    {
        _logger.LogInformation(message: "Messaging Chat Send interact");
        return View();
    }

    /// <summary>
    /// Displays the page for reading real-time messages.
    /// </summary>
    /// <returns>Returns the view for the real-time message reading page.</returns>
    [HttpGet(template: "read")]
    public IActionResult Read()
    {
        _logger.LogInformation(message: "Messaging Chat Read interact");
        return View();
    }

    /// <summary>
    /// Displays the page for reading the latest messages sent to the chat.
    /// </summary>
    /// <returns>Returns the view for the latest messages reading page.</returns>
    [HttpGet(template: "read-latest")]
    public IActionResult ReadLatest()
    {
        _logger.LogInformation(message: "Messaging Chat ReadLatest interact");
        return View();
    }
}
