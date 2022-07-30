#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Dto;
using Server.Models;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class PollController : ControllerBase
{
    private readonly ServerContext _context;

    public PollController(ServerContext context)
    {
        _context = context;
    }

    [HttpPost("Create")]
    public async Task<ActionResult> Create([Bind("Token,Options")] CreatePollDto createPoll)
    {
        var host = await _context.JukeboxHost.FindAsync(createPoll.Token);
        if (host is null)
            return NotFound();

        if (host.SessionKey == NumberGenerator.Empty)
            return NotFound();

        var f = await _context.PollOption.AnyAsync(p => p.SessionKey == host.SessionKey);
        if (await _context.PollOption.AnyAsync(p => p.SessionKey == host.SessionKey))
            return Unauthorized();

        foreach (var option in createPoll.Options)
        {
            PollOption pollOption = new PollOption
            {
                Option = option.Id,
                SessionKey = host.SessionKey,
                Name = option.Name,
                Votes = 0
            };

            await _context.AddAsync(pollOption);
        }

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("Remove")]
    public async Task<ActionResult> Remove([Bind("Token")] CreatePollDto createPoll)
    {
        var host = await _context.JukeboxHost.FindAsync(createPoll.Token);
        if (host is null)
            return NotFound();

        if (host.SessionKey == NumberGenerator.Empty)
            return NotFound();

        var pollOption = _context.PollOption.Where(option => option.SessionKey == host.SessionKey);
        if (pollOption.Count() <= 0)
            return Ok();

        _context.RemoveRange(pollOption);

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("Vote")]
    public async Task<ActionResult> Vote([Bind("Token,Options")] VotePollDto votePoll)
    {
        var client = await _context.JukeboxClient.FindAsync(votePoll.Token);
        if (client is null)
            return NotFound(false);

        if (client.SessionKey == NumberGenerator.Empty)
            return NotFound(false);

        var pollOption = await _context.PollOption.FirstOrDefaultAsync(option => option.SessionKey == client.SessionKey && option.Option == votePoll.OptionId);
        if (pollOption == null)
            return Unauthorized(false);

        var newPollOption = pollOption with
        {
            Votes = pollOption.Votes + 1,
        };

        _context.Entry(pollOption).State = EntityState.Deleted;

        _context.Update(newPollOption);

        await _context.SaveChangesAsync();

        return Ok(true);
    }

    [HttpPost("GetPoll")]
    public async Task<ActionResult<IEnumerable<PollOption>>> GetPoll([Bind("SessionKey")] SessionDto session)
    {
        if (session.SessionKey == NumberGenerator.Empty || session.SessionKey == null)
            return NotFound();

        // WARNING: Where method is not async
        var pollOption = _context.PollOption.Where(option => option.SessionKey == session.SessionKey);
        if (pollOption.Count() <= 0)
            return Unauthorized(null);

        return Ok(await pollOption.ToListAsync());
    }

    [HttpGet("All")]
    public async Task<IEnumerable<PollOption>> All()
    {
        return await _context.PollOption.ToListAsync();
    }

    [HttpPost("RemoveDev")]
    public async Task<ActionResult> RemoveDev(string sessionKey)
    {
        if (sessionKey == NumberGenerator.Empty)
            return NotFound();

        var pollOption = _context.PollOption.Where(option => option.SessionKey == sessionKey);
        if (pollOption.Count() <= 0)
            return Ok();

        _context.RemoveRange(pollOption);

        await _context.SaveChangesAsync();

        return Ok();
    }

}
