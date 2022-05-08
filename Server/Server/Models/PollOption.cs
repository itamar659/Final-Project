using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

public record PollOption
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("JukeboxSession")]
    public string SessionKey { get; set; }

    public int Option { get; set; }

    public string Name { get; set; }

    public int Votes { get; set; }

    public PollOption()
    {
        SessionKey = NumberGenerator.Empty;
        Name = string.Empty;

        Votes = 0;
    }
}
