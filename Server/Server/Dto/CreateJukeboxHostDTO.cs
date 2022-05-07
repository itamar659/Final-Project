using System.ComponentModel.DataAnnotations;

namespace Server.Dto;

public record CreateJukeboxHostDto
{
    [Key]
    public string Password { get; set; } = string.Empty;
}
