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

namespace Server.Controllers
{
    public class JukeboxHostsController : Controller
    {
        private readonly ServerContext _context;

        public JukeboxHostsController(ServerContext context)
        {
            _context = context;
        }

        // GET: JukeboxHosts
        public async Task<IActionResult> Index()
        {
            return View(await _context.JukeboxHost.ToListAsync());
        }

        // GET: JukeboxHosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jukeboxHost = await _context.JukeboxHost
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jukeboxHost == null)
            {
                return NotFound();
            }

            return View(jukeboxHost);
        }

        // GET: JukeboxHosts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JukeboxHosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Token,SessionKey")] JukeboxHost jukeboxHost)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jukeboxHost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jukeboxHost);
        }

        // GET: JukeboxHosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            return View(jukeboxHost);
        }

        // POST: JukeboxHosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Token,SessionKey")] JukeboxHost jukeboxHost)
        {
            if (id != jukeboxHost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jukeboxHost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JukeboxHostExists(jukeboxHost.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(jukeboxHost);
        }

        // GET: JukeboxHosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jukeboxHost = await _context.JukeboxHost
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jukeboxHost == null)
            {
                return NotFound();
            }

            return View(jukeboxHost);
        }

        // POST: JukeboxHosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jukeboxHost = await _context.JukeboxHost.FindAsync(id);
            _context.JukeboxHost.Remove(jukeboxHost);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JukeboxHostExists(int id)
        {
            return _context.JukeboxHost.Any(e => e.Id == id);
        }
    }
}
