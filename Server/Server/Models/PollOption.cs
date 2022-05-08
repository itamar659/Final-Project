using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

public record PollOption
{
    [ForeignKey("JukeboxSession")]
    public string SessionKey { get; set; }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    public string Name { get; set; }

    public int Votes { get; set; }

    public PollOption()
    {
        SessionKey = NumberGenerator.Empty;
        Name = string.Empty;

        Votes = 0;
    }
}
