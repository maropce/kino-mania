using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KinoMania2.Data;
using KinoMania2.Models;
using Microsoft.AspNetCore.Authorization;

namespace KinoMania2.Controllers
{
    public class FilmsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FilmsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Films


        public async Task<IActionResult> Index(int? filmTypeId)
        {
            IQueryable<Film> filmsQuery = _context.Film.Include(f => f.FilmType);

            // Filtrowanie po gatunku, jeśli filmTypeId zostało przekazane
            if (filmTypeId.HasValue)
            {
                filmsQuery = filmsQuery.Where(f => f.FilmTypeId == filmTypeId.Value);
            }

            var films = await filmsQuery.ToListAsync();

            ViewData["FilmTypes"] = new SelectList(await _context.FilmType.ToListAsync(), "Id", "Type");

            return View(films);
        }

        // GET: Films/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Film
                .Include(f => f.FilmType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // GET: Films/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {

            ViewData["FilmTypeId"] = new SelectList(_context.FilmType, "Id", "Type");
            return View();
        }

        // POST: Films/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,time,description,FilmTypeId")] Film film)
        {
            if (ModelState.IsValid)
            {
                _context.Add(film);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FilmTypeId"] = new SelectList(_context.FilmType, "Id", "Type", film.FilmTypeId);
            return View(film);
        }

        // GET: Films/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
           
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Film.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            ViewData["FilmTypeId"] = new SelectList(_context.FilmType, "Id", "Type", film.FilmTypeId);
            return View(film);
        }

        // POST: Films/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,time,description,FilmTypeId")] Film film)
        {
            if (id != film.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(film);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmExists(film.Id))
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
            ViewData["FilmTypeId"] = new SelectList(_context.FilmType, "Id", "Type", film.FilmTypeId);
            return View(film);
        }

        // GET: Films/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
           
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Film
                .Include(f => f.FilmType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var film = await _context.Film.FindAsync(id);
            if (film != null)
            {
                _context.Film.Remove(film);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmExists(int id)
        {
            return _context.Film.Any(e => e.Id == id);
        }
    }
}
