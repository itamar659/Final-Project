using Server;

namespace Server.Dto;

public class RoomUpdateDto
{
    public RoomUpdateDto()
    {
        Token = NumberGenerator.EmptyId;
        PinCode = NumberGenerator.EmptyPinCode;

        SongName = string.Empty;
        Position = TimeSpan.Zero;
        Duration = TimeSpan.Zero;
        IsPlaying = false;
    }

    /// <summary>
    /// a unique token to reference this host
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// a validation code to enter the room
    /// </summary>
    public string PinCode { get; set; }

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
