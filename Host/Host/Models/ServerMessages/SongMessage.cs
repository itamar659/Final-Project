namespace Host.Models.ServerMessages;
public class SongMessage
{
    public string SongName { get; set; }
    public double Duration { get; set; }
    public double Position { get; set; }
    public bool IsPlaying { get; set; }
}
