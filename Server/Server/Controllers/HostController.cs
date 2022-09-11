using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Dto;
using Server.Models;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class HostController : Controller
{
    private ServerContext _context;
    private RoomHubManager _hub;

    public HostController(ServerContext context, RoomHubManager hub)
    {
        _context = context;
        _hub = hub;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Get([Bind("Token")] JukeboxHost jukeboxHost)
    {
        var host = await _context.Hosts.FindAsync(jukeboxHost.Token);
        if (host is null)
            return NotFound();

        if (host.RoomId != NumberGenerator.EmptyId)
        {
            var room = await _context.Rooms.FindAsync(host.RoomId);
            if (room is not null)
                return Ok(host.MergeDtoWith(room));
        }

        return Ok(host);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Connect([Bind("Username")] JukeboxHost jukeboxHost)
    {
        var host = await _context.Hosts.FirstOrDefaultAsync(h => h.Username == jukeboxHost.Username);
        if (host is null)
            return NotFound();

        return Ok(host);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> OpenRoom([Bind("Token")] JukeboxHostEditDto jukeboxHost)
    {
        // get the host
        var host = await _context.Hosts.FindAsync(jukeboxHost.Token);
        if (host is null)
            return NotFound();

        try
        {
            JukeboxRoom? room;

            if (host.RoomId == NumberGenerator.EmptyId)
                room = await openRoomAsync(host);
            else
                room = await findRoom(host);

            if (room is null)
                // coudln't found the room or create new one
                return NoContent();

            //await _hub.JoinRoom(room.RoomId); // can't actually do it without connectionId

            return Ok(room);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (await _context.Hosts.FindAsync(jukeboxHost.Token) is null)
                return NotFound();

            throw;
        }
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> CloseRoom([Bind("Token,ConnectionId")] JukeboxHostEditDto jukeboxHost)
    {
        // get the host
        var host = await _context.Hosts.FindAsync(jukeboxHost.Token);
        if (host is null)
            return NotFound();

        try
        {
            if (host.RoomId == NumberGenerator.EmptyId)
                return Unauthorized();

            await clearHostRoomAsync(host);

            var room = await _context.Rooms.FindAsync(host.RoomId);
            if (room is not null)
            {
                // close and remove the room is needed
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();

                await removeRoomPollAsync(room);
                //await closeRoomAsync(host, room);

                // notify hub the room is closed
                //await _hub.LeaveRoom(room.RoomId);
                //await _hub.CloseRoom(room.RoomId);
            }

            return Ok();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (await _context.Hosts.FindAsync(jukeboxHost.Token) is null)
                return NotFound();

            throw;
        }
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> EditProfile([Bind("Token,Hostname,Summary,Description,BannerUrl,AvatarUrl")] JukeboxHostEditDto jukeboxHost)
    {
        // get the host
        var host = await _context.Hosts.FindAsync(jukeboxHost.Token);
        if (host is null)
            return NotFound();

        // create updated records
        JukeboxHost newHost = host with
        {
            Hostname = jukeboxHost.Hostname != String.Empty ? jukeboxHost.Hostname : host.Hostname,
            Summary = jukeboxHost.Summary != String.Empty ? jukeboxHost.Summary : host.Summary,
            Description = jukeboxHost.Description != String.Empty ? jukeboxHost.Description : host.Description,
            BannerUrl = jukeboxHost.BannerUrl != String.Empty ? jukeboxHost.BannerUrl : host.BannerUrl,
            AvatarUrl = jukeboxHost.AvatarUrl != String.Empty ? jukeboxHost.AvatarUrl : host.AvatarUrl
        };

        // update and save in database
        _context.Entry(host).State = EntityState.Detached;
        _context.Update(newHost);

        await _context.SaveChangesAsync();

        if (newHost.RoomId != NumberGenerator.EmptyId)
            // Notify the hub about the changes
            await _hub.UpdateHostProfile(newHost.RoomId, newHost.ToProfileDto());

        return Ok();
    }

    [HttpPost("admin_only/[action]")]
    public async Task<IEnumerable<JukeboxHost>> All()
    {
        return await _context.Hosts.ToListAsync();
    }

    [HttpPost("admin_only/[action]")]
    public async Task<ActionResult<JukeboxHost>> Create([Bind("Username")] JukeboxHost jukeboxHost)
    {
        if (jukeboxHost is null)
            return BadRequest();

        if (await _context.Hosts.AnyAsync(h => h.Username == jukeboxHost.Username))
            return Unauthorized();

        var newHost = await _context.AddAsync(new JukeboxHost() { Username = jukeboxHost.Username });

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { Token = newHost.Entity.Token }, newHost.Entity);
    }

    [HttpPost("admin_only/[action]")]
    public async Task<ActionResult> Delete(JukeboxHost jukeboxHost)
    {
        var host = await _context.Hosts.FirstOrDefaultAsync(h => h.Username == jukeboxHost.Username);
        if (host == null)
            return NotFound();

        _context.Remove(host);

        await _context.SaveChangesAsync();

        return NoContent();
    }




    private async Task<JukeboxRoom?> openRoomAsync(JukeboxHost host)
    {
        // generate room id
        var roomId = NumberGenerator.GenerateId();

        // create updated records
        JukeboxHost newHost = host with
        {
            RoomId = roomId
        };

        JukeboxRoom newRoom = new JukeboxRoom
        {
            RoomId = roomId,
        };

        // update and save in database
        _context.Entry(host).State = EntityState.Detached;
        _context.Update(newHost);

        var updated = await _context.Rooms.AddAsync(newRoom);

        await _context.SaveChangesAsync();

        return updated.Entity;
    }

    private async Task closeRoomAsync(JukeboxHost host, JukeboxRoom room)
    {
        // create updated records
        JukeboxHost newHost = host with
        {
            RoomId = NumberGenerator.EmptyId
        };

        // update and save in database
        _context.Entry(host).State = EntityState.Detached;
        _context.Rooms.Remove(room);
        _context.Update(newHost);

        await _context.SaveChangesAsync();
    }

    private async Task clearHostRoomAsync(JukeboxHost host)
    {
        // create updated records
        JukeboxHost newHost = host with
        {
            RoomId = NumberGenerator.EmptyId
        };

        // update and save in database
        _context.Entry(host).State = EntityState.Detached;
        _context.Update(newHost);
        await _context.SaveChangesAsync();
    }

    private async Task removeRoomPollAsync(JukeboxRoom room)
    {
        var pollOption = _context.Polls.Where(option => option.RoomId == room.RoomId);
        if (pollOption.Count() <= 0)
            return;

        foreach (var option in pollOption)
            _context.Remove(option);

        await _context.SaveChangesAsync();
    }

    private async Task<JukeboxRoom?> findRoom(JukeboxHost host)
    {
        return await _context.Rooms.FindAsync(host.RoomId);
    }
}
