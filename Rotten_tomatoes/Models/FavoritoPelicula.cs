namespace Rotten_tomatoes.Models
{
    public class FavoritoPelicula
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public int CinefiloId { get; set; }
        public Cinefilo Cinefilo { get; set; }
        public int PeliculaId { get; set; }
        public Pelicula Pelicula { get; set;}
    }
}
