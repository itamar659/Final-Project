using Server;
using Server.Models;

namespace Server.Dto;

public class RoomFullDto
{
    public RoomFullDto()
    {
        RoomId = NumberGenerator.EmptyId;
        PinCode = NumberGenerator.EmptyPinCode;
        OnlineUsers = 0;
        OpeningTime = DateTime.MinValue;

        Hostname = string.Empty;
        Summary = string.Empty;
        Description = string.Empty;
        BannerUrl = string.Empty;
        AvatarUrl = string.Empty;

        SongName = string.Empty;
        Position = TimeSpan.Zero;
        Duration = TimeSpan.Zero;
        IsPlaying = false;
    }

    // host details

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

    // room details

    /// <summary>
    /// A unique room id for each room
    /// </summary>
    public string RoomId { get; set; }

    /// <summary>
    /// a validation code to enter the room
    /// </summary>
    public string PinCode { get; set; }

    /// <summary>
    /// counter the users in the room
    /// </summary>
    public int OnlineUsers { get; set; }

    /// <summary>
    /// room opening time
    /// </summary>
    public DateTime OpeningTime { get; set; }

    // Song details:

    /// <summary>
    /// the name of the song that is now playing in the room
    /// </summary>
    public string SongName { get; set; }

    /// <summary>
    /// song position
    /// </summary>
    public TimeSpan Position { get; set; }

    /// <summary>
    /// song duration
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// indicates if the room playing songs atm
    /// </summary>
    public bool IsPlaying { get; set; }
}
