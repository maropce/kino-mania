using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KinoMania2.Models;

namespace KinoMania2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<KinoMania2.Models.Film> Film { get; set; } = default!;
        public DbSet<KinoMania2.Models.FilmType> FilmType { get; set; } = default!;
        public DbSet<KinoMania2.Models.Repositore> Repositore { get; set; } = default!;
        public DbSet<KinoMania2.Models.Seans> Seans { get; set; } = default!;
        public DbSet<KinoMania2.Models.Reservation> Reservation { get; set; } = default!;
    }
}
