using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

public record FavoriteSongs
{
    public FavoriteSongs()
    {
        Id = 0;
        SongName = string.Empty;
        Token = NumberGenerator.EmptyId;
    }

    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(JukeboxClient))]
    public string Token { get; set; }

    public string SongName { get; set; }
}