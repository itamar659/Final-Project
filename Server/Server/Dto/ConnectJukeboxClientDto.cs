using System.ComponentModel.DataAnnotations;

namespace Server.Dto;
public class ConnectJukeboxClientDto
{
    [Key]
    public string Password { get; set; } = string.Empty;

}
