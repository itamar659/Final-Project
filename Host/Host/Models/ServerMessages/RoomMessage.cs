namespace Host.Models.ServerMessages;
public class RoomMessage
{
    public RoomMessage()
    {
        Hostname = string.Empty;
        Summary = string.Empty;
        Description = string.Empty;
        BannerUrl = string.Empty;
        AvatarUrl = string.Empty;

        RoomId = string.Empty;
        PinCode = string.Empty;
        OnlineUsers = 0;
        OpeningTime = DateTime.MinValue;

        SongName = string.Empty;
        Position = TimeSpan.Zero;
        Duration = TimeSpan.Zero;
        IsPlaying = false;
    }

    // host details

    public string Hostname { get; set; }

    public string Summary { get; set; }

    public string Description { get; set; }

    public string BannerUrl { get; set; }

    public string AvatarUrl { get; set; }

    // room details

    public string RoomId { get; set; }

    public string PinCode { get; set; }

    public int OnlineUsers { get; set; }

    public DateTime OpeningTime { get; set; }

    // song details

    public string SongName { get; set; }

    public TimeSpan Position { get; set; }

    public TimeSpan Duration { get; set; }

    public bool IsPlaying { get; set; }
}
