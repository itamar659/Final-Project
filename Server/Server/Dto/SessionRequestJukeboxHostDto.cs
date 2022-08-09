using System.ComponentModel.DataAnnotations;

namespace Server.Dto;

public class SessionRequestJukeboxHostDto
{
    [Key]
    public string Token { get; set; } = string.Empty;

    public string PinCode { get; set; } = string.Empty;

    public string OwnerName { get; set; } = string.Empty;

}
