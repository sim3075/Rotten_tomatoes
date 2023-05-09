namespace Rotten_tomatoes.Models
{
    public class FavoritoTvShow
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public int CinefiloId { get; set; }
        public Cinefilo Cinefilo { get; set; }
        public int TvShowId { get; set; }
        public TvShow TvShow { get; set; }

    }
}
