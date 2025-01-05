using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace KinoMania2.Models
{
    public class Reservation
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Użytkownik")]
        [Required(ErrorMessage = "Użytkownik jest wymagany.")]
        public string? UserId { get; set; }

        [Display(Name = "Użytkownik")]
        public IdentityUser? User { get; set; }


        [Display(Name = "Seans")]
        [Required(ErrorMessage = "Seans jest wymagany.")]
        public int SeansId { get; set; }

        [Display(Name = "Seans")]
        public virtual Seans? Seans { get; set; }
    }
}
