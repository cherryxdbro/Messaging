namespace Messaging.Models;

/// <summary>
/// Represents a chat message, containing information about the user, the message text, and the time it was sent.
/// </summary>
public class Message
{
    /// <summary>
    /// Unique identifier for the message.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Name of the message sender. This field is required.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Text of the message sent by the user. This field is required.
    /// </summary>
    public required string Text { get; set; }

    /// <summary>
    /// The time when the message was received.
    /// </summary>
    public DateTime? Timestamp { get; set; }
}
