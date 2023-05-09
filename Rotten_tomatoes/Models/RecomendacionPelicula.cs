namespace Rotten_tomatoes.Models
{
    public class RecomendacionPelicula
    {
        public int Id { get; set; }

        public string Url { get; set; }
        public int ExpertoId { get; set; }
        public ExpertoEnCine ExpertoEnCine { get; set; }
        public int PeliculaId { get; set; }
        public Pelicula Pelicula { get; set;}
    }
}
