namespace Server.Models;

public record JukeboxHost
{
    public int Id { get; set; }
    public string? Token { get; set; }
    public string? SessionKey { get; set; }
}
