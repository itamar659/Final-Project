using System.ComponentModel.DataAnnotations;

namespace Server.Dto;

public class EditJukeboxHostDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string? Token { get; set; }
}
