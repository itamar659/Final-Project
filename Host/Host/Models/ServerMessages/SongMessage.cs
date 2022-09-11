namespace Host.Models.ServerMessages;
public class SongMessage
{
    public string SongName { get; set; }
    public TimeSpan Duration { get; set; }
    public TimeSpan Position { get; set; }
    public bool IsPlaying { get; set; }
}
