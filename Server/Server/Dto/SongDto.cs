using System.ComponentModel.DataAnnotations;

namespace Server.Dto;

public class SongDto
{
    [Key]
    public string Token { get; set; }
    public string SongName { get; set; }
    public double Duration { get; set; }
    public double Position { get; set; }
    public bool IsPaused { get; set; }
}
