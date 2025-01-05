using System.ComponentModel.DataAnnotations;

namespace KinoMania2.Models
{
	public class Seans
	{
		[Display(Name = "ID")]
		public int Id { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane!")]
        [Display(Name = "Data rozpoczęcia")]

		[DataType(DataType.DateTime)]
		public DateTime Start { get; set; }

		[Display(Name = "Film")]
        [Required(ErrorMessage = "To pole jest wymagane!")]
        public int FilmId { get; set; }

		[Display(Name = "Film")]
		public virtual Film? Film { get; set; }
	}
}
