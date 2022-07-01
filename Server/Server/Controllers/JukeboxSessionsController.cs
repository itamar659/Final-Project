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
public class JukeboxSessionsController : ControllerBase
{
    private readonly ServerContext _context;

    public JukeboxSessionsController(ServerContext context)
    {
        _context = context;
    }

    // TODO: Should be use request the token too in otder to follow one requests over time.
    [HttpPost("GetSession")]
    public async Task<ActionResult<JukeboxSessionDto>> GetSession([Bind("SessionKey")] SessionDto sessionDto)
    {
        var session = await _context.FindAsync<JukeboxSession>(sessionDto.SessionKey);
        if (session == null)
            return NotFound();

        return Ok(session.ToDto());
    }

    [HttpPost("AvailableSessions")]
    public async Task<ActionResult<List<JukeboxSessionDto>>> AvailableSessions()
    {
        return Ok(await _context.JukeboxSession
            .Select(s => s.ToDto())
            .ToListAsync());
    }

    // TODO: For testing only
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return Ok(await _context.JukeboxSession.ToListAsync());
    }
}
