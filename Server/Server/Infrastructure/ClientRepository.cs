using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Dto;
using Server.Models;

namespace Server.Infrastructure;

public class ClientRepository : IClientRepository
{
    private ServerContext _context;

    public ClientRepository(ServerContext context)
    {
        _context = context;
    }

    public Task JoinRoomAsync(JukeboxClientJoinRoomDto jukeboxClient)
    {
        throw new NotImplementedException();
    }

    public Task LeaveRoomAsync(JukeboxClientJoinRoomDto jukeboxClient)
    {
        throw new NotImplementedException();
    }

    public async Task<JukeboxClient?> CreateAsync(JukeboxClient jukeboxClient)
    {
        return (await _context.AddAsync(
            new JukeboxClient() {
                Username = jukeboxClient.Username,
                IsAnonymous = jukeboxClient.IsAnonymous
            }
        )).Entity;
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task<JukeboxClient?> GetClientByTokenAsync(string token)
    {
        return await _context.Clients.FindAsync(token);
    }

    public async Task<JukeboxClient?> GetClientByUsernameAsync(string username, bool isAnonymous)
    {
        return await _context.Clients.FirstOrDefaultAsync(c => c.Username == username && c.IsAnonymous == isAnonymous);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Update(JukeboxClient jukeboxClient, JukeboxClient newClient)
    {
        _context.Entry(jukeboxClient).State = EntityState.Detached;
        _context.Update(newClient);
    }

    public Task<List<JukeboxClient>> GetAllClients()
    {
        return _context.Clients.ToListAsync();
    }

    public void Remove(JukeboxClient jukeboxClient)
    {
        _context.Remove(jukeboxClient);
    }
}
