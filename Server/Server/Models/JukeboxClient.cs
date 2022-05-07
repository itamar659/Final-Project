using System.ComponentModel.DataAnnotations;

namespace Server.Models;

public record JukeboxClient
{
    [Key]
    public string Token { get; set; }

    public string SessionKey { get; set; }

    public JukeboxClient()
    {
        Token = NumberGenerator.Generate();
        SessionKey = NumberGenerator.Empty;
    }
}
