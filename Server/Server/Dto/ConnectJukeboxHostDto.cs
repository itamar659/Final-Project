using System.ComponentModel.DataAnnotations;

namespace Server.Dto;

public record ConnectJukeboxHostDto
{
    [Key]
    public string Password { get; set; } = string.Empty;
}
