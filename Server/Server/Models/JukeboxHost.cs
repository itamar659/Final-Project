﻿using System.ComponentModel.DataAnnotations;

namespace Server.Models;

public record JukeboxHost
{
    [Key]
    public string Token { get; set; }

    [Required]
    public string Password { get; set; }

    public string SessionKey { get; set; }

    public JukeboxHost(string password)
    {
        Password = password;

        Token = NumberGenerator.Generate();
        SessionKey = NumberGenerator.Empty;
    }
}
