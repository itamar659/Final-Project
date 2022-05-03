using System.ComponentModel.DataAnnotations;

namespace Server.Dto;

public class EditJukeboxHost
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string? Token { get; set; }
}
