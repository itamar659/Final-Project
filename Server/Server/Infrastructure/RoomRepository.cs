using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;

namespace Server.Infrastructure;

public class RoomRepository : IRoomRepository
{
    private ServerContext _context;
    //TODO: Complete the repository and use it...
    public RoomRepository(ServerContext context)
    {
        _context = context;
    }

    public async Task<JukeboxRoom?> GetRoomByIdAsync(string roomId)
    {
        return await _context.Rooms.FindAsync(roomId);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Update(JukeboxRoom room, JukeboxRoom newRoom)
    {
        _context.Entry(room).State = EntityState.Detached;
        _context.Update(newRoom);
    }
    public void Dispose()
    {
        _context.Dispose();
    }
}
