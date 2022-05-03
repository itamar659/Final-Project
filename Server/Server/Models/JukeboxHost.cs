namespace Server.Models;

public class JukeboxHost
{
    public int Id { get; set; }
    public string? Token { get; set; }
    public string? SessionKey { get; set; }
}
