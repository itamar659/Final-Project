using System.ComponentModel.DataAnnotations;

namespace Server.Dto;
public class LoginJukeboxClientDto
{
    [Key]
    public string Password { get; set; } = string.Empty;

}
