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
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(await _context.Reservation
                .Include(s => s.Seans)
                .Include(s => s.User)
                .Include(r => r.Seans.Film)
                .Where(s => s.UserId == userId)
                .ToArrayAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var reservation = await _context.Reservation
                .Include(s => s.Seans)
                .Include(s => s.User)
                .Include(r => r.Seans.Film)
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId); // Sprawdź, czy rekord należy do użytkownika

            if (reservation == null)
            {
                return Forbid(); 
            }

            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        [HttpGet]
        public JsonResult GetDatesByFilmId(int filmId)
        {
            var dates = _context.Seans
                .Where(s => s.FilmId == filmId) // Filtruj seanse po Id filmu
                .Select(s => s.Start)            // Pobierz tylko daty
                .Distinct()                     // Usuń duplikaty
                .ToList();

            return Json(dates);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {

            ViewData["Film"] = new SelectList(_context.Film, "Id", "Title");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string userId, int filmId, DateTime date)
        {
            // Znajdź odpowiedni seans na podstawie filmId i daty
            var seans = await _context.Seans
                .FirstOrDefaultAsync(s => s.FilmId == filmId && s.Start == date);


            if (seans == null)
            {
                ModelState.AddModelError("", "Nie odnaleziono daty seansu do podanego filmu!.");
                ViewData["Film"] = new SelectList(_context.Film, "Id", "Title", filmId);
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", userId);
                return View();
            }
            if (date <= DateTime.Now)
            {
                ModelState.AddModelError("", "Za późno! Wybierz późniejszą datę :(");
                ViewData["Film"] = new SelectList(_context.Film, "Id", "Title", filmId);
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", userId);
                return View();
            }
            // Utwórz nową rezerwację
            var reservation = new Reservation
            {
                UserId = userId,
                SeansId = seans.Id
            };

            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Film"] = new SelectList(_context.Film, "Id", "Title", filmId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", userId);
            return View(reservation);
        }


        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var reservation = await _context.Reservation
                 .Include(s => s.Seans)
                .Include(s => s.User)
                .Include(r => r.Seans.Film)
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId); // Sprawdź, czy rekord należy do użytkownika

            if (reservation == null)
            {
                return Forbid(); 
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservation.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservation.Any(e => e.Id == id);
        }
    }
}
