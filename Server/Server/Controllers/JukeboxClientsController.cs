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
using Server.Services;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class JukeboxClientsController : ControllerBase
{
    private readonly ServerContext _context;

    private readonly JukeboxSessionRequestHandler _jukeboxSessionRequestHandler;

    public JukeboxClientsController(ServerContext context)
    {
        _context = context;

        _jukeboxSessionRequestHandler = new JukeboxSessionRequestHandler(context);
    }

    [HttpGet(Name = "JukeboxClient")]
    public async Task<ActionResult<JukeboxClient>> JukeboxClient(string token)
    {
        var jukeboxHost = await _context.JukeboxClient.FindAsync(token);

        if (jukeboxHost == null)
            return NotFound();

        return Ok(jukeboxHost);
    }

    [HttpPost("Login")]
    public async Task<ActionResult<JukeboxClientDto>> Login([Bind("Password")] LoginJukeboxClientDto client)
    {
        var jukeboxClient = await _context.JukeboxClient.FirstOrDefaultAsync(h => h.Password == client.Password);
        if (jukeboxClient == null)
            return NotFound();

        return Ok(jukeboxClient.ToDto());
    }

    [HttpPost("AnonymousLogin")]
    public async Task<ActionResult<JukeboxClientDto>> AnonymousLogin([Bind("Username")] AnonymousLoginJukeboxClientDto client)
    {
        // TODO: Change the stupid logic

        if (client is null)
            return BadRequest();

        var jukeboxClient = await _context.JukeboxClient.FirstOrDefaultAsync(c => c.Password == client.Username);
        if (jukeboxClient != null)
            return Ok(jukeboxClient.ToDto());

        var newClient = await _context.AddAsync(new JukeboxClient(client.Username));

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(JukeboxClient), new { Token = newClient.Entity.Token }, newClient.Entity);
    }

    [HttpPost("Create")]
    public async Task<ActionResult<JukeboxClientDto>> Create([Bind("Password")] LoginJukeboxClientDto client)
    {
        if (client is null)
            return BadRequest();

        var newClient = await _context.AddAsync(new JukeboxClient(client.Password));

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(JukeboxClient), new { Token = newClient.Entity.Token }, newClient.Entity);
    }

    [HttpPost("JoinSession")]
    public async Task<ActionResult<JukeboxClient>> JoinSession([Bind("Token,OwnerName")] SessionRequestJukeboxClientDto jukeboxClient)
    {
        if (!await _context.JukeboxClient.AnyAsync(c => c.Token == jukeboxClient.Token))
            return NotFound();

        var session = await _context.JukeboxSession
            .Where(h => h.OwnerName == jukeboxClient.OwnerName)
            .FirstOrDefaultAsync();

        if (session == null)
            return NotFound();

        var client = await _context.FindAsync<JukeboxClient>(jukeboxClient.Token);

        try
        {
            if (client.SessionKey != NumberGenerator.Empty)
                await _jukeboxSessionRequestHandler.LeaveSessionAsync(client);

            await _jukeboxSessionRequestHandler.JoinSessionAsync(client, session);
            return Ok(session.ToDto());
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.JukeboxClient.AnyAsync(c => c.Token == jukeboxClient.Token))
                return NotFound();

            if (!await _context.JukeboxSession.AnyAsync(s => s.OwnerName == jukeboxClient.OwnerName))
                return NotFound();

            throw;
        }
    }

    [HttpPost("LeaveSession")]
    public async Task<ActionResult<JukeboxClient>> LeaveSession([Bind("Token")] SessionRequestJukeboxClientDto jukeboxClient)
    {
        if (!await _context.JukeboxClient.AnyAsync(c => c.Token == jukeboxClient.Token))
            return NotFound();

        var client = await _context.FindAsync<JukeboxClient>(jukeboxClient.Token);

        try
        {
            await _jukeboxSessionRequestHandler.LeaveSessionAsync(client);
            return Ok();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.JukeboxClient.AnyAsync(c => c.Token == jukeboxClient.Token))
                return NotFound();

            throw;
        }
    }

    // TODO: TESTING PURPOSES
    [HttpGet("All")]
    public async Task<IEnumerable<JukeboxClient>> All()
    {
        var clients = await _context.JukeboxClient.ToListAsync();

        return clients;
    }

    // TODO: TESTING PURPOSES
    [HttpPost("Delete")]
    public async Task<ActionResult> DeleteConfirmed(JukeboxClient client)
    {
        var jukeboxClient = await _context.JukeboxClient.FirstOrDefaultAsync(c => c.Token == client.Token);
        if (jukeboxClient == null)
            return NotFound();

        _context.Remove(jukeboxClient);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
