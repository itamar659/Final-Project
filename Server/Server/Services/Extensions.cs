using Server.Dto;
using Server.Models;

namespace Server.Services;

public static class Extensions
{
    public static JukeboxHostDTO ToDTO(this JukeboxHost host)
    {
        return new JukeboxHostDTO
        {
            Id = host.Id,
            Token = host.Token,
            SessionKey = host.SessionKey
        };
    }
}