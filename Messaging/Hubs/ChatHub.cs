using Microsoft.AspNetCore.SignalR;

namespace Messaging.Hubs;

/// <summary>
/// Hub for managing real-time messaging interactions.
/// Provides functionality to broadcast messages to all connected clients.
/// </summary>
/// <param name="logger">Instance of ILogger for logging activities in the Hub.</param>
public class ChatHub(ILogger<ChatHub> logger) : Hub
{
    private readonly ILogger<ChatHub> _logger = logger;

    /// <summary>
    /// Invoked when a new connection is established.
    /// Logs the connection event with the client's connection ID.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation(
            message: "Client connected. Connection ID: {ConnectionId}",
            args: Context.ConnectionId
        );
        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Invoked when a connection is closed.
    /// Logs the disconnection event with the client's connection ID.
    /// </summary>
    /// <param name="exception">An optional exception that caused the disconnection.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation(
            message: "Client disconnected. Connection ID: {ConnectionId}",
            args: Context.ConnectionId
        );
        await base.OnDisconnectedAsync(exception);
    }
}
