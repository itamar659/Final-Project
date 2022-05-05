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

    public JukeboxHostsController(ServerContext context)
    {
        _context = context;
    }

    [HttpGet(Name = "JukeboxHosts")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResult<JukeboxHostDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JukeboxHostDto>> GetJukeboxHost(string token)
    {
        var jukeboxHost = await _context.JukeboxHost.FirstOrDefaultAsync(host => host.Token == token);
        //var jukeboxHost = await _context.JukeboxHost.FindAsync(id);

        if (jukeboxHost == null)
            return NotFound();

        return Ok(jukeboxHost.ToDTO());
    }

    // TODO: For testing only! should not be available to the outside world
    [HttpGet("All")]
    public async Task<IEnumerable<JukeboxHostDto>> All()
    {
        var hosts = await _context.JukeboxHost.ToListAsync();

        return hosts.Select(host => host.ToDTO());
    }

    // TODO: TESTING PERPUCES
    [HttpPost("Create")]
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //[ValidateAntiForgeryToken]
    public async Task<ActionResult<JukeboxHostDto>> Create([Bind("Token")] CreateJukeboxHostDto host)
    {
        if (!ModelState.IsValid)
        {
            return Unauthorized();
        }

        if (host == null)
        {
            return BadRequest();
        }

        if (JukeboxHostExists(host.Token))
        {
            return NoContent();
        }

        var newHost = _context.Add(new JukeboxHost
        {
            Token = host.Token,
        });

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetJukeboxHost), new { Token = newHost.Entity.Token }, newHost.Entity.ToDTO());
    }

    [HttpPost("GenerateSessionKey")]
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //[ValidateAntiForgeryToken]
    public async Task<ActionResult<JukeboxHostDto>> GenerateSessionKey([Bind("Id,Token")] EditJukeboxHostDto jukeboxHost)
    {
        if (!ModelState.IsValid)
        {
            return Unauthorized();
        }

        if (!JukeboxHostExists(jukeboxHost.Id, jukeboxHost.Token))
        {
            return NotFound();
        }

        try
        {
            JukeboxHost newHost = new JukeboxHost
            {
                Id = jukeboxHost.Id,
                Token = jukeboxHost.Token,
                SessionKey = SessionKeyGenerator.Generate()
            };

            _context.Update(newHost);

            await _context.SaveChangesAsync();

            return newHost.ToDTO();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!JukeboxHostExists(jukeboxHost.Id, jukeboxHost.Token))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
    }

    [HttpPost("Delete")]
    //[ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(EditJukeboxHostDto host)
    {
        var jukeboxHost = await _context.JukeboxHost.FindAsync(host.Id);

        if (jukeboxHost == null)
        {
            return NotFound();
        }

        if (host.Token != jukeboxHost.Token)
        {
            return Unauthorized();
        }

        _context.JukeboxHost.Remove(jukeboxHost);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool JukeboxHostExists(int id)
    {
        return _context.JukeboxHost.Any(e => e.Id == id);
    }

    private bool JukeboxHostExists(string token)
    {
        return _context.JukeboxHost.Any(e => e.Token == token);
    }

    private bool JukeboxHostExists(int id, string token)
    {
        return _context.JukeboxHost.Any(e => e.Id == id && e.Token == token);
    }
}
