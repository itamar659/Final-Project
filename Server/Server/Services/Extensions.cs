using Server.Dto;
using Server.Models;

namespace Server.Services;

public static class Extensions
{
    public static JukeboxHostDto ToDto(this JukeboxHost host)
    {
        return new JukeboxHostDto
        {
            Token = host.Token,
            SessionKey = host.SessionKey
        };
    }
    public static JukeboxClientDto ToDto(this JukeboxClient client)
    {
        return new JukeboxClientDto
        {
            Token = client.Token,
            SessionKey = client.SessionKey
        };
    }

    public static JukeboxSessionDto ToDto(this JukeboxSession session)
    {
        return new JukeboxSessionDto
        {
            SessionKey = session.SessionKey,
            OwnerName = session.OwnerName,
            ActiveUsers = session.ActiveUsers,
            TotalUsers = session.TotalUsers,
            SongName = session.SongName,
            SongDuration = session.SongDuration,
            SongPosition = session.SongPosition,
        };
    }
}