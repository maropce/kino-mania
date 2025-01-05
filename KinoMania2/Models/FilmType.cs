using System.ComponentModel.DataAnnotations;

namespace KinoMania2.Models
{
    public class FilmType
    {
        public int Id { get; set; }

        [Display(Name = "Gatunek Filmy")]
        [Required(ErrorMessage = "To pole jest wymagane!")]
        [StringLength(30, ErrorMessage = "Długość typu nie może przekaraczać 30 znaków")]
        public string Type { get; set; }
    }
}
