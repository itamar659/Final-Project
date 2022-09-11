using System.ComponentModel.DataAnnotations;

namespace Server.Models;

public record JukeboxHost
{
    public JukeboxHost()
    {
        Token = NumberGenerator.GenerateId();
        Username = string.Empty;
        RoomId = NumberGenerator.EmptyId;
        Hostname = string.Empty;

        Summary = string.Empty;
        Description = string.Empty;
        BannerUrl = string.Empty;
        AvatarUrl = string.Empty;
    }

    /// <summary>
    /// a unique token to reference this host
    /// </summary>
    [Key]
    public string Token { get; set; }

    /// <summary>
    /// the username to connect the server - credential
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// this host room id
    /// </summary>
    public string RoomId { get; set; }

    /// <summary>
    /// friendly unique name for display
    /// </summary>
    public string Hostname { get; set; }

    /// <summary>
    /// host summary
    /// </summary>
    public string Summary { get; set; }

    /// <summary>
    /// host description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// a banner image url
    /// </summary>
    public string BannerUrl { get; set; }

    /// <summary>
    /// an avatar image url
    /// </summary>
    public string AvatarUrl { get; set; }
}