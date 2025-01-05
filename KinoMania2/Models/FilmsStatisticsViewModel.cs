namespace KinoMania2.Models
{
    public class FilmStatisticsViewModel
    {
        public int TotalMinutesWatched { get; set; }
        public Dictionary<string, int> FilmsWatchedByGenre { get; set; }
    }

}
