namespace Rotten_tomatoes.Models
{
    public class RecomendacionTvShow
    {
        public int Id { get; set; }
        public int ExpertoId { get; set; }
        public ExpertoEnCine ExpertoEnCine { get; set; }
        public int TvShowId { get; set; }
        public TvShow TvShow { get; set;}
    }
}
