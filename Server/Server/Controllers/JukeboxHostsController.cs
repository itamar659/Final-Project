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
    public async Task<IEnumerable<JukeboxHostDTO>> GetJukeboxHost()
    {
        var hosts = await _context.JukeboxHost.ToListAsync();

        return hosts.Select(host => host.ToDTO());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<JukeboxHostDTO>> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var jukeboxHost = await _context.JukeboxHost.FindAsync(id);

        if (jukeboxHost == null)
        {
            return NotFound();
        }

        return jukeboxHost.ToDTO();
    }

    [HttpPost("Create")]
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //[ValidateAntiForgeryToken]
    public async Task<ActionResult<JukeboxHostDTO>> Create([Bind("Token")] CreateJukeboxHostDTO host)
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

        return CreatedAtAction(nameof(GetJukeboxHost), new { Id = newHost.Entity.Id }, newHost.Entity.ToDTO());
    }

    [HttpPost("GenerateSessionKey")]
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //[ValidateAntiForgeryToken]
    public async Task<ActionResult<JukeboxHostDTO>> GenerateSessionKey([Bind("Id,Token")] EditJukeboxHost jukeboxHost)
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
    public async Task<ActionResult> DeleteConfirmed(EditJukeboxHost host)
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
