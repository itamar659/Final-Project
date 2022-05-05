using System.ComponentModel.DataAnnotations;

namespace Server.Dto;

public record JukeboxHostDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string? Token { get; set; }

    public string? SessionKey { get; set; }
}
