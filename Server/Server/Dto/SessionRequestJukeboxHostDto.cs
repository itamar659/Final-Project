using System.ComponentModel.DataAnnotations;

namespace Server.Dto;

public class SessionRequestJukeboxHostDto
{
    [Key]
    public string Token { get; set; } = string.Empty;
}
