using Server.Dto;
using Server.Models;

namespace Server.Infrastructure;

public interface IClientRepository : IDisposable
{
    Task<JukeboxClient?> GetClientByTokenAsync(string token);

    Task<JukeboxClient?> GetClientByUsernameAsync(string username, bool isAnonymous);

    Task<List<JukeboxClient>> GetAllClients();

    Task JoinRoomAsync(JukeboxClientJoinRoomDto jukeboxClient);

    Task LeaveRoomAsync(JukeboxClientJoinRoomDto jukeboxClient);

    Task<JukeboxClient?> CreateAsync(JukeboxClient jukeboxClient);

    void Update(JukeboxClient jukeboxClient, JukeboxClient newClient);

    void Remove(JukeboxClient jukeboxClient);

    Task SaveAsync();
}
