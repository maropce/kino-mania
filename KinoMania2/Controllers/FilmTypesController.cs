using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KinoMania2.Data;
using KinoMania2.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace KinoMania2.Controllers
{
    public class FilmTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FilmTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FilmTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.FilmType.ToListAsync());
        }

        // GET: FilmTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filmType = await _context.FilmType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (filmType == null)
            {
                return NotFound();
            }

            return View(filmType);
        }

        // GET: FilmTypes/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
           
            return View();
        }

        // POST: FilmTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type")] FilmType filmType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(filmType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(filmType);
        }

        // GET: FilmTypes/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
           
            if (id == null)
            {
                return NotFound();
            }

            var filmType = await _context.FilmType.FindAsync(id);
            if (filmType == null)
            {
                return NotFound();
            }
            return View(filmType);
        }

        // POST: FilmTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type")] FilmType filmType)
        {
            if (id != filmType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(filmType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmTypeExists(filmType.Id))
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
            return View(filmType);
        }

        // GET: FilmTypes/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
           
            if (id == null)
            {
                return NotFound();
            }

            var filmType = await _context.FilmType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (filmType == null)
            {
                return NotFound();
            }

            return View(filmType);
        }

        // POST: FilmTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var filmType = await _context.FilmType.FindAsync(id);
            if (filmType != null)
            {
                _context.FilmType.Remove(filmType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmTypeExists(int id)
        {
            return _context.FilmType.Any(e => e.Id == id);
        }
    }
}
