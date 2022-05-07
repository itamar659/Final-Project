using System.ComponentModel.DataAnnotations;

namespace Server.Dto;

public class SessionJukeboxHostDto
{
    [Key]
    public string Token { get; set; } = string.Empty;
}
