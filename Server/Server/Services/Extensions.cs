using Server.Dto;
using Server.Models;

namespace Server.Services;

public static class Extensions
{
    public static JukeboxHostDto ToDTO(this JukeboxHost host)
    {
        return new JukeboxHostDto
        {
            Id = host.Id,
            Token = host.Token,
            SessionKey = host.SessionKey
        };
    }
}