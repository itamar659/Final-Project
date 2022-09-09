namespace Host.Models;

public class HostProfile
{
    public HostProfile()
    {
        Token = string.Empty;
        Hostname = string.Empty;
        RoomId = string.Empty;
        Summary = string.Empty;
        Description = string.Empty;
        BannerUrl = string.Empty;
        AvatarUrl = string.Empty;
    }

    public string Token { get; set; }

    public string Hostname { get; set; }

    public string RoomId { get; set; }

    public string Summary { get; set; }

    public string Description { get; set; }

    public string BannerUrl { get; set; }

    public string AvatarUrl { get; set; }
}