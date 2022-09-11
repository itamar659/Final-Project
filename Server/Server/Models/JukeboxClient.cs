using System.ComponentModel.DataAnnotations;

namespace Server.Models;

public record JukeboxClient
{
    public JukeboxClient()
    {
        Token = NumberGenerator.GenerateId();
        Username = string.Empty;
        IsAnonymous = true;
        AvatarUrl = string.Empty;
        RoomId = NumberGenerator.EmptyId;
    }

    /// <summary>
    /// a unique token to reference this client
    /// </summary>
    [Key]
    public string Token { get; set; }

    /// <summary>
    /// the username to connect the server
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// the room id the client connects to
    /// </summary>
    public string RoomId { get; set; }

    /// <summary>
    /// is the client used an anonymous login or not.
    /// </summary>
    public bool IsAnonymous { get; set; }

    /// <summary>
    /// an avatar image url
    /// </summary>
    public string AvatarUrl { get; set; }
}