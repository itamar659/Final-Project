﻿using System.ComponentModel.DataAnnotations;

namespace Server.Dto;

public record JukeboxHostDto
{
    [Key]
    public string Token { get; set; } = string.Empty;

    public string? SessionKey { get; set; }
}
