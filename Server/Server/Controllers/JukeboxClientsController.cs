﻿#nullable disable
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

    [HttpGet(Name = "JukeboxClients")]
    public async Task<ActionResult<JukeboxClient>> GetJukeboxHost(string token)
    {
        var jukeboxHost = await _context.JukeboxClient.FindAsync(token);

        if (jukeboxHost == null)
            return NotFound();

        return Ok(jukeboxHost);
    }

    [HttpPost("Connect")]
    public async Task<ActionResult<JukeboxClientDto>> Connect([Bind("Password")] ConnectJukeboxClientDto client)
    {
        var jukeboxClient = await _context.JukeboxClient.FirstOrDefaultAsync(h => h.Password == client.Password);
        if (jukeboxClient == null)
            return NotFound();

        return Ok(jukeboxClient.ToDto());
    }

    [HttpPost("Create")]
    public async Task<ActionResult<JukeboxClientDto>> Create([Bind("Password")] ConnectJukeboxClientDto client)
    {
        if (client is null)
            return BadRequest();

        if (await _context.JukeboxClient.AnyAsync(c => c.Password == client.Password))
            return Unauthorized();

        var newClient = await _context.AddAsync(new JukeboxClient(client.Password));

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetJukeboxHost), new { Token = newClient.Entity.Token }, newClient.Entity);
    }

    [HttpPost("JoinSession")]
    public async Task<ActionResult<JukeboxClient>> JoinSession([Bind("Token,SessionKey")] SessionRequestJukeboxClientDto jukeboxClient)
    {
        if (!await _context.JukeboxClient.AnyAsync(c => c.Token == jukeboxClient.Token))
            return NotFound();

        if (!await _context.JukeboxSession.AnyAsync(s => s.SessionKey == jukeboxClient.SessionKey))
            return NotFound();

        if (jukeboxClient.SessionKey == NumberGenerator.Empty)
            return NotFound();

        var client = await _context.FindAsync<JukeboxClient>(jukeboxClient.Token);
        var session = await _context.FindAsync<JukeboxSession>(jukeboxClient.SessionKey);

        try
        {
            await _jukeboxSessionRequestHandler.JoinSessionAsync(client, session);
            return Ok();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.JukeboxClient.AnyAsync(c => c.Token == jukeboxClient.Token))
                return NotFound();

            if (!await _context.JukeboxSession.AnyAsync(s => s.SessionKey == jukeboxClient.SessionKey))
                return NotFound();

            throw;
        }
    }

    [HttpPost("LeaveSession")]
    public async Task<ActionResult<JukeboxClient>> LeaveSession([Bind("Token,SessionKey")] SessionRequestJukeboxClientDto jukeboxClient)
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

            if (!await _context.JukeboxSession.AnyAsync(s => s.SessionKey == jukeboxClient.SessionKey))
                return NotFound();

            if (jukeboxClient.SessionKey == NumberGenerator.Empty)
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
