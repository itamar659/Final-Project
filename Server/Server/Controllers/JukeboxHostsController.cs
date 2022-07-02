#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Dto;
using Server.Models;
using Server.Services;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class JukeboxHostsController : ControllerBase
{
    private readonly ServerContext _context;

    private readonly JukeboxSessionRequestHandler _jukeboxSessionRequestHandler;

    public JukeboxHostsController(ServerContext context)
    {
        _context = context;

        _jukeboxSessionRequestHandler = new JukeboxSessionRequestHandler(context);
    }

    [HttpGet(Name = "JukeboxHosts")]
    // TODO: Add ProducesResponseType as needed to each method
    //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResult<JukeboxHostDto>))]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JukeboxHostDto>> GetJukeboxHost(string token)
    {
        var jukeboxHost = await _context.JukeboxHost.FindAsync(token);
        if (jukeboxHost == null)
            return NotFound();

        return Ok(jukeboxHost.ToDto());
    }

    [HttpPost("Connect")]
    public async Task<ActionResult<JukeboxHostDto>> Connect([Bind("Password")] ConnectJukeboxHostDto host)
    {
        var jukeboxHost = await _context.JukeboxHost.FirstOrDefaultAsync(h => h.Password == host.Password);
        if (jukeboxHost == null)
            return NotFound();

        return Ok(jukeboxHost.ToDto());
    }

    [HttpPost("OpenSession")]
    // TODO: Add valudation to post requests
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //[ValidateAntiForgeryToken]
    public async Task<ActionResult<JukeboxSessionDto>> OpenSession([Bind("Token")] SessionRequestJukeboxHostDto jukeboxHost)
    {
        //if (!ModelState.IsValid)
        //    return Unauthorized();

        var host = await _context.JukeboxHost.FindAsync(jukeboxHost.Token);
        if (host is null)
            return NotFound();

        try
        {
            JukeboxSession session = await _jukeboxSessionRequestHandler.OpenSessionAsync(host);
            if (session is null)
                return NoContent();

            return Ok(session.ToDto());
        }
        catch (DbUpdateConcurrencyException)
        {
            if (await _context.JukeboxHost.FindAsync(jukeboxHost.Token) is null)
                return NotFound();

            throw;
        }
    }

    [HttpPost("ChangeSong")]
    public async Task<ActionResult<JukeboxSessionDto>> ChangeSong([Bind("Token,SongName,Duration,Position")] SongDto songDto)
    {
        var host = await _context.JukeboxHost.FindAsync(songDto.Token);
        if (host is null)
            return NotFound();

        JukeboxSession session = await _jukeboxSessionRequestHandler.GetSessionAsync(host.SessionKey);
        if (session is null)
            return NotFound();

        JukeboxSession updatedSession = session with
        {
            SongName = songDto.SongName,
            SongDuration = songDto.Duration,
            SongPosition = songDto.Position
        };

        _context.Entry(session).State = EntityState.Detached;

        _context.Update(updatedSession);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("CloseSession")]
    public async Task<ActionResult<JukeboxSessionDto>> CloseSession([Bind("Token")] SessionRequestJukeboxHostDto jukeboxHost)
    {
        // TODO: Remove the active polls

        var host = await _context.JukeboxHost.FindAsync(jukeboxHost.Token);
        if (host is null)
            return NotFound();

        try
        {
            await _jukeboxSessionRequestHandler.CloseSession(host);
            return Ok();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (await _context.JukeboxHost.FindAsync(jukeboxHost.Token) is null)
                return NotFound();

            throw;
        }
    }

    [HttpPost("ChangePinCode")]
    public async Task<ActionResult<bool>> ChangePinCode([Bind("Token,PinCode")] SessionRequestJukeboxHostDto jukeboxHost)
    {
        var host = await _context.JukeboxHost.FindAsync(jukeboxHost.Token);
        if (host is null)
            return NotFound(false);

        try
        {
            await _jukeboxSessionRequestHandler.ChangePinCode(host, jukeboxHost.PinCode);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (await _context.JukeboxHost.FindAsync(jukeboxHost.Token) is null)
                return NotFound(false);

            if (await _context.JukeboxSession.FindAsync(host.SessionKey) is null)
                return NotFound(false);

            throw;
        }

        return Ok(true);
    }

    // TODO: TESTING PURPOSES
    [HttpGet("All")]
    public async Task<IEnumerable<JukeboxHostDto>> All()
    {
        var hosts = await _context.JukeboxHost.ToListAsync();

        return hosts.Select(host => host.ToDto());
    }

    // TODO: TESTING PURPOSES
    [HttpPost("Create")]
    public async Task<ActionResult<JukeboxHostDto>> Create([Bind("Password")] ConnectJukeboxHostDto host)
    {
        if (host is null)
            return BadRequest();

        if (await _context.JukeboxHost.AnyAsync(h => h.Password == host.Password))
            return Unauthorized();

        var newHost = await _context.AddAsync(new JukeboxHost(host.Password));

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetJukeboxHost), new { Token = newHost.Entity.Token }, newHost.Entity.ToDto());
    }

    // TODO: TESTING PURPOSES
    [HttpPost("Delete")]
    public async Task<ActionResult> DeleteConfirmed(ConnectJukeboxHostDto host)
    {
        var jukeboxHost = await _context.JukeboxHost.FirstOrDefaultAsync(h => h.Password == host.Password);
        if (jukeboxHost == null)
            return NotFound();

        _context.Remove(jukeboxHost);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
