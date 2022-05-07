using System.ComponentModel.DataAnnotations;

namespace Server.Dto;

public record SessionRequestJukeboxClientDto
{
    [Key]
    public string Token { get; set; } = string.Empty;

    public string SessionKey { get; set; } = NumberGenerator.Empty;
}
