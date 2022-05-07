#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;

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

    // TODO: For testing only
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return Ok(await _context.JukeboxSession.ToListAsync());
    }
}
