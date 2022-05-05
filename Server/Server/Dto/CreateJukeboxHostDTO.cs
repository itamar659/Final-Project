using System.ComponentModel.DataAnnotations;

namespace Server.Dto;

public record CreateJukeboxHostDto
{
    [Required]
    public string? Token { get; set; }
}
