using Server.Models;

namespace Server.Infrastructure;

public interface IRoomRepository : IDisposable
{
    Task<JukeboxRoom?> GetRoomByIdAsync(string roomId);

    void Update(JukeboxRoom room, JukeboxRoom newRoom);

    Task SaveAsync();
}
