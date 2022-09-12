using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

public record JukeboxRoom
{
    public JukeboxRoom()
    {
        RoomId = NumberGenerator.EmptyId;
        PinCode = NumberGenerator.EmptyPinCode;
        OnlineUsers = 0;
        OpeningTime = DateTime.Now;

        SongName = string.Empty;
        Position = TimeSpan.Zero;
        Duration = TimeSpan.Zero;
        IsPlaying = false;
    }

    /// <summary>
    /// A unique room id for each room
    /// </summary>
    [Key]
    [ForeignKey(nameof(JukeboxHost))]
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