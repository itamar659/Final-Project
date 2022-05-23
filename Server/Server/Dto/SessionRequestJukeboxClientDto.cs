using System.ComponentModel.DataAnnotations;

namespace Server.Dto;

public record SessionRequestJukeboxClientDto
{
    [Key]
    public string Token { get; set; } = string.Empty;

    [Required]
    public string OwnerName { get; set; }
}
