namespace Server;

public class Song
{
    public Song()
    {
        SongName = string.Empty;
        Duration = TimeSpan.Zero;
        Position = TimeSpan.Zero;
        IsPlaying = false;
    }

    public string SongName { get; set; }

    public TimeSpan Duration { get; set; }

    public TimeSpan Position { get; set; }

    public bool IsPlaying { get; set; }
}