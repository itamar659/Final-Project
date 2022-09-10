namespace Host.Models.ServerMessages;
public class RoomMessage
{
    public RoomMessage()
    {
        RoomId = string.Empty;
        Hostname = string.Empty;
        PinCode = string.Empty;
        OnlineUsers = 0;
        OpeningTime = DateTime.MinValue;

        SongName = string.Empty;
        Position = TimeSpan.Zero;
        Duration = TimeSpan.Zero;
        IsPlaying = false;
    }

    public string RoomId { get; set; }

    public string Hostname { get; set; }

    public string PinCode { get; set; }

    public int OnlineUsers { get; set; }

    public DateTime OpeningTime { get; set; }

    public string SongName { get; set; }

    public TimeSpan Position { get; set; }

    public TimeSpan Duration { get; set; }

    public bool IsPlaying { get; set; }
}
