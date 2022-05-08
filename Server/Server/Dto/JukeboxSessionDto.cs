namespace Server.Dto;

public class JukeboxSessionDto
{
    public string SessionKey { get; set; }
    public string? OwnerName { get; set; }

    public int ActiveUsers { get; set; }

    public int TotalUsers { get; set; }

    public string? SongName { get; set; }

    public double SongDuration { get; set; }

    public double SongPosition { get; set; }

}
