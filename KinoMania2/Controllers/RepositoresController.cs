using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KinoMania2.Data;
using KinoMania2.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace KinoMania2.Controllers
{
    [Authorize]
    public class RepositoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RepositoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Repositores
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(await _context.Repositore
                .Include(s => s.Film)
                .Include(s => s.User)
                .Where(s => s.UserId == userId)
                .ToArrayAsync());
        }

        // GET: Repositores/Create
        public IActionResult Create()
        {
            
            ViewData["FilmId"] = new SelectList(_context.Film, "Id", "Title");
            return View();
        }

        // POST: Repositores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FilmId,UserId")] Repositore repositore)
        {
            if (ModelState.IsValid)
            {
                _context.Add(repositore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FilmId"] = new SelectList(_context.Film, "Id", "Title", repositore.FilmId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", repositore.UserId);
            return View(repositore);
        }

        // GET: Repositores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var repositore = await _context.Repositore
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId); // Sprawdź, czy rekord należy do użytkownika

            if (repositore == null)
            {
                return Forbid(); 
            }

            
            if (repositore == null)
            {
                return NotFound();
            }
            ViewData["FilmId"] = new SelectList(_context.Film, "Id", "Title", repositore.FilmId);
            return View(repositore);
        }

        // POST: Repositores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FilmId,UserId")] Repositore repositore)
        {
            if (id != repositore.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(repositore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepositoreExists(repositore.Id))
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
            ViewData["FilmId"] = new SelectList(_context.Film, "Id", "Title", repositore.FilmId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", repositore.UserId);
            return View(repositore);
        }

        // GET: Repositores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var repositore = await _context.Repositore
                .Include(s => s.User)
                .Include(r => r.Film)
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId); // Sprawdź, czy rekord należy do użytkownika

            if (repositore == null)
            {
                return Forbid();
            }

            return View(repositore);
        }

        // POST: Repositores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var repositore = await _context.Repositore.FindAsync(id);
            if (repositore != null)
            {
                _context.Repositore.Remove(repositore);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RepositoreExists(int id)
        {
            return _context.Repositore.Any(e => e.Id == id);
        }
    }
}
