using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

public record JukeboxSession
{
    [Key]
    [ForeignKey("JukeboxHost")]
    public string SessionKey { get; set; }

    public string PinCode { get; set; }

    public string? OwnerName { get; set; }

    public int ActiveUsers { get; set; }

    public int TotalUsers { get; set; }

    public string? SongName { get; set; }

    public double SongDuration { get; set; }

    public double SongPosition { get; set; }

    public bool IsPaused { get; set; }

    public JukeboxSession()
    {
        SessionKey = NumberGenerator.Empty;
        PinCode = string.Empty;

        OwnerName = "[OWNER_NAME]";
    }
}
