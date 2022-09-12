using Server.Dto;
using Server.Infrastructure;
using Server.Models;

namespace Server;

public static class Extensions
{
    public static WebApplicationBuilder AddCustomServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IClientRepository, ClientRepository>();
        builder.Services.AddTransient<IRoomRepository, RoomRepository>();

        builder.Services.AddTransient<RoomHubManager>();


        return builder;
    }

    public static JukeboxHostProfileDto ToProfileDto(this JukeboxHost host)
    {
        return new JukeboxHostProfileDto
        {
            Hostname = host.Hostname,
            Summary = host.Summary,
            Description = host.Description,
            AvatarUrl = host.AvatarUrl,
            BannerUrl = host.BannerUrl,
        };
    }

    public static RoomFullDto MergeDtoWith(this JukeboxHost host, JukeboxRoom room)
    {
        return mergeHostRoomDto(host, room);
    }

    public static RoomFullDto MergeDtoWith(this JukeboxRoom room, JukeboxHost host)
    {
        return mergeHostRoomDto(host, room);
    }

    private static RoomFullDto mergeHostRoomDto(JukeboxHost host, JukeboxRoom room)
    {
        return new RoomFullDto
        {
            // host details
            Hostname = host.Hostname,
            Summary = host.Summary,
            Description = host.Description,
            AvatarUrl = host.AvatarUrl,
            BannerUrl = host.BannerUrl,

            // room details:
            RoomId = room.RoomId,
            PinCode = room.PinCode,
            OnlineUsers = room.OnlineUsers,
            OpeningTime = room.OpeningTime,
            SongName = room.SongName,
            Duration = room.Duration,
            Position = room.Position,
            IsPlaying = room.IsPlaying,
        };
    }
}
