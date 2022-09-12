using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Dto;
using System.ComponentModel;
using Server.Data;
using Server.Models;

namespace Server.Controllers;

//[Authorize]
[ApiController]
[Route("[controller]")]
public class RoomController : Controller
{
    private ServerContext _context;
    private RoomHubManager _hub;

    public RoomController(ServerContext context, RoomHubManager hub)
    {
        _context = context;
        _hub = hub;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Get([Bind("Token,RoomId")] JukeboxRoom jukeboxRoom)
    {
        var room = await _context.Rooms.FindAsync(jukeboxRoom.RoomId);
        if (room is null)
            return NotFound();

        var host = await _context.Hosts.FirstAsync(h => h.RoomId == room.RoomId);
        if (host is null)
            return NotFound();

        return Ok(room.MergeDtoWith(host));
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<List<RoomFullDto>>> OpenRooms()
    {
        // TODO: Use a DbQuery -> A view that contains the host and the room already.
        List<RoomFullDto> rooms = new List<RoomFullDto>();
        foreach (var room in _context.Rooms.ToList())
        {
            var host = await _context.Hosts.FirstOrDefaultAsync(h => h.RoomId == room.RoomId);
            if (host is not null)
                rooms.Add(host.MergeDtoWith(room));
        }

        return Ok(rooms);
    }

    [TypeConverter(typeof(string))]
    [HttpPost("[action]")]
    public async Task<IActionResult> ChangePinCode([Bind("Token")] RoomUpdateDto jukeboxHost)
    {
        // get the host
        var host = await _context.Hosts.FindAsync(jukeboxHost.Token);
        if (host is null)
            return NotFound();

        if (host.RoomId == NumberGenerator.EmptyId)
            return Unauthorized();

        var room = await _context.Rooms.FindAsync(host.RoomId);
        if (room is null)
            return NotFound();

        try
        {
            var pinCode = NumberGenerator.GeneratePinCode();
            await changePinCodeAsync(room, pinCode);

            return Ok(new { pinCode = pinCode});
        }
        catch (DbUpdateConcurrencyException)
        {
            if (await _context.Hosts.FindAsync(jukeboxHost.Token) is null)
                return NotFound();

            if (await _context.Rooms.FindAsync(host.RoomId) is null)
                return NotFound();

            throw;
        }
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> UpdateSong([Bind("Token,SongName,Duration,Position,IsPlaying")] RoomUpdateDto song)
    {
        // get the host
        var host = await _context.Hosts.FindAsync(song.Token);
        if (host is null)
            return NotFound();

        if (host.RoomId == NumberGenerator.EmptyId)
            return Unauthorized();

        var room = await _context.Rooms.FindAsync(host.RoomId);
        if (room is null)
            return NotFound();

        // create updated records
        var newRoom = room with
        {
            SongName = song.SongName,
            Duration = song.Duration,
            Position = song.Position,
            IsPlaying = song.IsPlaying
        };

        // update and save in database
        _context.Entry(room).State = EntityState.Detached;
        _context.Update(newRoom);

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> CreatePoll([Bind("Token,Options")] PollCreationDto poll)
    {
        var host = await _context.Hosts.FindAsync(poll.Token);
        if (host is null)
            return NotFound();

        if (host.RoomId == NumberGenerator.EmptyId)
            return NotFound();

        var room = await _context.Rooms.FindAsync(host.RoomId);
        if (room is null)
            return NotFound();

        var pollOption = _context.Polls.Where(option => option.RoomId == room.RoomId);
        if (pollOption.Count() > 0)
            return Unauthorized();

        foreach (var option in poll.Options)
        {
            var op = option with
            {
                PollId = option.PollId,
                RoomId = room.RoomId,
                SongName = option.SongName,
                Votes = option.Votes,
            };

            _context.Polls.Add(op);
        }

        await _context.SaveChangesAsync();

        await _hub.CreatePoll(room.RoomId, poll.Options);

        return Ok(room);
    }


    [HttpPost("[action]")]
    public async Task<ActionResult> RemovePoll([Bind("Token")] PollCreationDto poll)
    {
        var host = await _context.Hosts.FindAsync(poll.Token);
        if (host is null)
            return NotFound();

        if (host.RoomId == NumberGenerator.EmptyId)
            return NotFound();

        var room = await _context.Rooms.FindAsync(host.RoomId);
        if (room is null)
            return NotFound();

        var pollOption = _context.Polls.Where(option => option.RoomId == room.RoomId);
        if (pollOption.Count() <= 0)
            return Ok();

        // update and save in database
        foreach (var option in pollOption)
            _context.Remove(option);

        await _context.SaveChangesAsync();

        //_roomHubManager.PollRemoved(room.RoomId); no need I think

        return Ok();
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> Vote([Bind("Token,PollId")] PollVoteDto poll)
    {
        var client = await _context.Clients.FindAsync(poll.Token);
        if (client is null)
            return NotFound();

        if (client.RoomId == NumberGenerator.EmptyId)
            return NotFound();

        var room = await _context.Rooms.FindAsync(client.RoomId);
        if (room is null)
            return NotFound();

        var options = _context.Polls.Where(option => option.RoomId == room.RoomId);
        if (options.Count() <= 0)
            return NotFound();

        var pollOption = options.First(o => o.PollId == poll.PollId);

        var newPollOption = pollOption with
        {
            Votes = pollOption.Votes + 1
        };

        // update and save in database
        _context.Entry(pollOption).State = EntityState.Detached;
        _context.Update(newPollOption);

        await _context.SaveChangesAsync();

        options = _context.Polls.Where(option => option.RoomId == room.RoomId);

        await _hub.UpdatePollVotes(room.RoomId, await options.ToListAsync());

        return Ok();
    }

    [HttpPost("[action]")]
    public ActionResult GetPoll([Bind("RoomId")] JukeboxRoom room)
    {
        if (room.RoomId == NumberGenerator.EmptyId)
            return NotFound();

        var pollOption = _context.Polls.Where(option => option.RoomId == room.RoomId);
        if (pollOption.Count() <= 0)
            return NotFound();

        return Ok(pollOption);
    }


    [HttpPost("admin_only/[action]")]
    public async Task<IEnumerable<JukeboxRoom>> All()
    {
        return await _context.Rooms.ToListAsync();
    }

    [HttpPost("admin_only/[action]")]
    public async Task<IEnumerable<PollOption>> AllPolls()
    {
        return await _context.Polls.ToListAsync();
    }

    [HttpPost("admin_only/[action]")]
    public async Task<IActionResult> DetelePoll(List<int> ids)
    {
        foreach (var id in ids)
        {
            var op = _context.Polls.Find(id);
            _context.Polls.Remove(op);
        }

        await _context.SaveChangesAsync();

        return Ok();
    }

    private async Task changePinCodeAsync(JukeboxRoom room, string pinCode)
    {
        // create updated records
        JukeboxRoom newRoom = room with
        {
            PinCode = pinCode
        };

        // update and save in database
        _context.Entry(room).State = EntityState.Detached;
        _context.Update(newRoom);

        await _context.SaveChangesAsync();
    }
}