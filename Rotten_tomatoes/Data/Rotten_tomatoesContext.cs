using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rotten_tomatoes.Models;

namespace Rotten_tomatoes.Data
{
    public class Rotten_tomatoesContext : DbContext
    {
        public Rotten_tomatoesContext (DbContextOptions<Rotten_tomatoesContext> options)
            : base(options)
        {
        }

        public DbSet<Rotten_tomatoes.Models.Pelicula> Pelicula { get; set; } = default!;

        public DbSet<Rotten_tomatoes.Models.TvShow>? TvShow { get; set; }

        public DbSet<Rotten_tomatoes.Models.TopPeliculas>? TopPeliculas { get; set; }

        public DbSet<Rotten_tomatoes.Models.TopTvShow>? TopTvShow { get; set; }
    }
}
