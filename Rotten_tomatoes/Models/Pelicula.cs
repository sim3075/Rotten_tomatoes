namespace Rotten_tomatoes.Models
{
    public class Pelicula
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Titulo { get; set; }
        public string? Img { get; set; }
        public string? Calificacion_critica { get; set; }
        public string? Calificacion_audiencia { get; set;}
        public string? Sinopsis { get; set; }
        public string? plataformas { get; set; }
        public string? Genero { get; set; }
        public string? Premier { get; set;}
        public string? Duracion { get; set; }

    }
}
