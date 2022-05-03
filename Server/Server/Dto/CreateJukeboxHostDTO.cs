using System.ComponentModel.DataAnnotations;

namespace Server.Dto;

public record CreateJukeboxHostDTO
{
    [Required]
    public string? Token { get; set; }
}
