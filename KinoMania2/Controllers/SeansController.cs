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

namespace KinoMania2.Controllers
{
    public class SeansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SeansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Seans
        public async Task<IActionResult> Index(DateTime? filterDate)
        {
            IQueryable<Seans> applicationDbContext = _context.Seans.Include(s => s.Film);

            // Filtrowanie po dacie, jeśli jest ustawiona
            if (filterDate.HasValue)
            {
                applicationDbContext = applicationDbContext.Where(s => s.Start.Date == filterDate.Value.Date);
            }

            var seansList = await applicationDbContext.ToListAsync();
            return View(seansList);
        }

        // GET: Seans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seans = await _context.Seans
                .Include(s => s.Film)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seans == null)
            {
                return NotFound();
            }

            return View(seans);
        }

        // GET: Seans/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["FilmId"] = new SelectList(_context.Film, "Id", "Title");
            return View();
        }

        // POST: Seans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Start,FilmId")] Seans seans)
        {
            if (ModelState.IsValid)
            {
                _context.Add(seans);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FilmId"] = new SelectList(_context.Film, "Id", "Id", seans.FilmId);
            return View(seans);
        }

        // GET: Seans/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seans = await _context.Seans.FindAsync(id);
            if (seans == null)
            {
                return NotFound();
            }
            ViewData["FilmId"] = new SelectList(_context.Film, "Id", "Title", seans.FilmId);
            return View(seans);
        }

        // POST: Seans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Start,FilmId")] Seans seans)
        {
            if (id != seans.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(seans);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeansExists(seans.Id))
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
            ViewData["FilmId"] = new SelectList(_context.Film, "Id", "Title", seans.FilmId);
            return View(seans);
        }

        // GET: Seans/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seans = await _context.Seans
                .Include(s => s.Film)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seans == null)
            {
                return NotFound();
            }

            return View(seans);
        }

        // POST: Seans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seans = await _context.Seans.FindAsync(id);
            if (seans != null)
            {
                _context.Seans.Remove(seans);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeansExists(int id)
        {
            return _context.Seans.Any(e => e.Id == id);
        }
    }
}
