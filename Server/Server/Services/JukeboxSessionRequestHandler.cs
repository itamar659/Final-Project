using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;

namespace Server.Services;

public class JukeboxSessionRequestHandler
{
    private readonly ServerContext _context;

    public JukeboxSessionRequestHandler(ServerContext context)
    {
        _context = context;
    }

    public async Task<JukeboxSession?> OpenSessionAsync(JukeboxHost host)
    {
        JukeboxSession? session;
        if (host.SessionKey != NumberGenerator.Empty)
        {
            session = await GetSessionAsync(host.SessionKey);
            if (session is not null)
            {
                return session;
            }
        }

        var key = NumberGenerator.Generate();

        JukeboxHost newHost = host with
        {
            SessionKey = key,
        };

        JukeboxSession newSession = new JukeboxSession
        {
            SessionKey = key,
        };

        _context.Entry(host).State = EntityState.Detached;

        _context.Update(newHost);
        var updated = await _context.AddAsync(newSession);

        await _context.SaveChangesAsync();

        return updated.Entity;
    }

    public async Task CloseSession(JukeboxHost host)
    {
        if (host.SessionKey == NumberGenerator.Empty)
            return;

        var session = await GetSessionAsync(host.SessionKey);
        if (session is null)
            return;

        JukeboxHost newHost = host with
        {
            SessionKey = NumberGenerator.Empty,
        };

        _context.Entry(host).State = EntityState.Detached;

        _context.Remove(session);
        _context.Update(newHost);

        await _context.SaveChangesAsync();
    }

    public async Task ChangePinCode(JukeboxHost host, string pinCode)
    {
        if (host.SessionKey == NumberGenerator.Empty)
            return;

        var session = await GetSessionAsync(host.SessionKey);
        if (session is null)
            return;

        JukeboxSession newSession = session with
        {
            PinCode = pinCode,
        };

        _context.Entry(session).State = EntityState.Detached;

        _context.Update(newSession);

        await _context.SaveChangesAsync();
    }

    public async Task ChangeOwnerName(JukeboxHost host, string name)
    {
        if (host.SessionKey == NumberGenerator.Empty)
            return;

        var session = await GetSessionAsync(host.SessionKey);
        if (session is null)
            return;

        JukeboxSession newSession = session with
        {
            OwnerName = name,
        };

        _context.Entry(session).State = EntityState.Detached;

        _context.Update(newSession);

        await _context.SaveChangesAsync();
    }

    public async Task JoinSessionAsync(JukeboxClient client, JukeboxSession session)
    {
        if (client.SessionKey != NumberGenerator.Empty)
            return;

        JukeboxSession newSession = session with
        {
            ActiveUsers = session.ActiveUsers + 1,
            TotalUsers = session.TotalUsers + 1,
        };

        JukeboxClient newClient = client with
        {
            SessionKey = session.SessionKey,
        };

        _context.Entry(session).State = EntityState.Detached;
        _context.Entry(client).State = EntityState.Detached;

        _context.Update(newSession);
        _context.Update(newClient);

        await _context.SaveChangesAsync();
    }

    public async Task LeaveSessionAsync(JukeboxClient client)
    {
        if (client.SessionKey == NumberGenerator.Empty)
            return;

        var session = await GetSessionAsync(client.SessionKey);
        if (session is not null)
        {
            JukeboxSession newSession = session with
            {
                ActiveUsers = session.ActiveUsers - 1,
            };

            _context.Entry(session).State = EntityState.Detached;

            _context.Update(newSession);
        }

        JukeboxClient newClient = client with
        {
            SessionKey = NumberGenerator.Empty,
        };

        _context.Entry(client).State = EntityState.Detached;

        _context.Update(newClient);

        await _context.SaveChangesAsync();
    }

    public async Task<JukeboxSession?> GetSessionAsync(string sessionKey)
    {
        return await _context.JukeboxSession.FindAsync(sessionKey);
    }
}
