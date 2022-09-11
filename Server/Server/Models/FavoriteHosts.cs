using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

public record FavoriteHosts
{
    public FavoriteHosts()
    {
        Id = 0;
        Hostname = string.Empty;
        Token = NumberGenerator.EmptyId;
    }

    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(JukeboxClient))]
    public string Token { get; set; }

    [ForeignKey(nameof(JukeboxHost))]
    public string Hostname { get; set; }
}