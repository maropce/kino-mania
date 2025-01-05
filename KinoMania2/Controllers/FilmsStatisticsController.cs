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
    [Authorize]
    public class FilmsStatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FilmsStatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: FilmsStatistics
        public async Task<IActionResult> Index()
        {
          
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            
            // Łączna liczba obejrzanych minut dla użytkownika
            var totalMinutesWatched = await _context.Reservation
                .Include(r => r.Seans)
                .ThenInclude(s => s.Film)
                .Where(r => r.UserId == userId) 
                .SumAsync(r => r.Seans.Film.time);

            // Ilość obejrzanych filmów z danego gatunku dla użytkownika
            var filmsWatchedByGenre = await _context.Reservation
                .Include(r => r.Seans)
                .ThenInclude(s => s.Film)
                .Where(r => r.UserId == userId)
                .GroupBy(r => r.Seans.Film.FilmType.Type)
                .Select(g => new { Genre = g.Key, Count = g.Count() })
                .ToListAsync();

            // Tworzenie modelu do widoku
            var statistics = new FilmStatisticsViewModel
            {
                TotalMinutesWatched = totalMinutesWatched,
                FilmsWatchedByGenre = filmsWatchedByGenre.ToDictionary(g => g.Genre, g => g.Count)
            };

            return View(statistics);
        }

    }
}
