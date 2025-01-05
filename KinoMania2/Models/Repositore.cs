using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace KinoMania2.Models
{
    public class Repositore
    {
        [Display(Name = "ID")]
        public int Id { get; set; }


        [Display(Name = "Film")]
        [Required(ErrorMessage = "To pole jest wymagane!")]
        public int FilmId { get; set; }

        [Display(Name = "Film")]
        public virtual Film? Film { get; set; }

        [Display(Name = "Użytkownik")]
        [Required(ErrorMessage = "To pole jest wymagane!")]
        public string? UserId { get; set; }

        [Display(Name = "Użytkownik")]
        public IdentityUser? User { get; set; }

    }
}

