namespace Client.Models;

public class PollOption
{
    public int Option { get; set; }
    public string Name { get; set; }
    public int Votes { get; set; }
    public int Timestamp { get; set; }
}