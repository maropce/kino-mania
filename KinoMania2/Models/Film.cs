using Microsoft.VisualBasic.FileIO;
using System;
using System.ComponentModel.DataAnnotations;



namespace KinoMania2.Models
{

    public class Film
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Tytuł")]
        [StringLength(100, ErrorMessage = "Długość tytułu musi być w zakresie od 1 do 100 znaków.")]
        [Required(ErrorMessage = "To pole jest wymagane!")]
        public string Title { get; set; }

        [Display(Name = "Autor")]
        [StringLength(100, ErrorMessage = "Długość nazwy autora musi być w zakresie od 1 do 100 znaków.")]
        [Required(ErrorMessage = "To pole jest wymagane!")]
        public string Author { get; set; }

        [Display(Name = "Długość filmu (minuty)")]
        [Range(1, 1000, ErrorMessage = "Długość filmu musi być w zakresie od 1 do 1000 minut.")]
        [Required(ErrorMessage = "To pole jest wymagane!")]
        public int time { get; set; }

        [Display(Name = "Opis")]
        [StringLength(500, ErrorMessage = "Długość opisu musi być w zakresie od 1 do 500 znaków.")]
        [Required(ErrorMessage = "To pole jest wymagane!")]
        public string description { get; set; }


        [Display(Name = "Gatunek")]
        [Required(ErrorMessage = "To pole jest wymagane!")]
        public int FilmTypeId { get; set; }

        [Display(Name = "Gatunek")]
        public virtual FilmType? FilmType { get; set; }
    }
}
