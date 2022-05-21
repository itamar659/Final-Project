using System.ComponentModel.DataAnnotations;

namespace Server.Models;

public record JukeboxClient
{
    [Key]
    public string Token { get; set; }

    [Required]
    public string Password { get; set; }

    public string SessionKey { get; set; }

    public JukeboxClient(string password)
    {
        Password = password;

        Token = NumberGenerator.Generate();
        SessionKey = NumberGenerator.Empty;
    }
}
