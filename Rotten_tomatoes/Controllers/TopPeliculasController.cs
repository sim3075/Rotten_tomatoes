using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rotten_tomatoes.Data;
using Rotten_tomatoes.Models;

namespace Rotten_tomatoes.Controllers
{
    public class TopPeliculasController : Controller
    {
        private readonly Rotten_tomatoesContext _context;

        public TopPeliculasController(Rotten_tomatoesContext context)
        {
            _context = context;
        }

        // GET: TopPeliculas
        public async Task<IActionResult> Index()
        {
              return _context.TopPeliculas != null ? 
                          View(await _context.TopPeliculas.ToListAsync()) :
                          Problem("Entity set 'Rotten_tomatoesContext.TopPeliculas'  is null.");
        }

        // GET: TopPeliculas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TopPeliculas == null)
            {
                return NotFound();
            }

            var topPeliculas = await _context.TopPeliculas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (topPeliculas == null)
            {
                return NotFound();
            }

            return View(topPeliculas);
        }

        // GET: TopPeliculas/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Tops()
        {
            var url_pelicula = scrape_top();
            foreach(var url in url_pelicula)
            {
                var pelicula = scrape_pelicula(url);
                _context.Add(pelicula);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        List<string> scrape_top()
        {
            async Task<string> call_url(string fullUrl)
            {
                HttpClient client = new HttpClient();
                var response = await client.GetStringAsync(fullUrl);
                return response;
            }

            List<string> parse_html(string html)
            {
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                var full_top = htmlDoc.DocumentNode.Descendants("a")
                    .Where(node => node.GetAttributeValue("class", "").Contains("dynamic-text-list__tomatometer-group"))
                    .ToList();

                List<string> urls_peliculas = new List<string>();
                foreach (HtmlNode nodo in full_top)
                {
                    urls_peliculas.Add(nodo.GetAttributeValue("href", ""));
                }

                return urls_peliculas;
            }

            string url = "https://www.rottentomatoes.com/";
            var response = call_url(url).Result;
            List<string> url_peliculas = parse_html(response);
            return url_peliculas;
        }



        public static Pelicula scrape_pelicula(string url)
        {
            //Accediendo a url
            async Task<string> call_url(string fullUrl)
            {
                HttpClient client = new HttpClient();
                var response = await client.GetStringAsync(fullUrl);
                return response;
            }

            //parseando datos
            Pelicula parse_html(string html, string url)
            {
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);


                string titulo = htmlDoc.DocumentNode.Descendants("img")
                .Where(node => node.ParentNode.GetAttributeValue("class", "").Contains("thumbnail")).
                ToList().First().GetAttributeValue("alt", " ").Remove(0, 18);

                string img = htmlDoc.DocumentNode.Descendants("img")
                .Where(node => node.ParentNode.GetAttributeValue("class", "").Contains("thumbnail")).
                ToList().First().GetAttributeValue("src", " ").Trim();

                string calificacion_critica = htmlDoc.DocumentNode.Descendants("score-board")
                .Where(node => node.GetAttributeValue("id", "").Contains("scoreboard"))
                .ToList().First().GetAttributeValue("tomatometerscore", "");

                string calificacion_audiencia = htmlDoc.DocumentNode.Descendants("score-board")
                .Where(node => node.GetAttributeValue("id", "").Contains("scoreboard"))
                .ToList().First().GetAttributeValue("audiencescore", "");

                string sinopsis = htmlDoc.DocumentNode.Descendants("p")
                .Where(node => node.GetAttributeValue("data-qa", "").
                Contains("movie-info-synopsis")).ToList().First().InnerText.Trim();

                var plataformas = htmlDoc.DocumentNode.Descendants("where-to-watch-meta")
                .Where(node => node.GetAttributeValue("data-qa", "").Contains("affiliate-item"))
                .ToList();

                string plataforma = "";
                foreach (var node in plataformas)
                {
                    plataforma += node.GetAttributeValue("affiliate", "-") + "";

                }

                string genero = "";
                string duracion = "";
                string premier = "";

                var info_pelicula = htmlDoc.DocumentNode.Descendants("li")
               .Where(node => node.GetAttributeValue("class", "").Contains("info-item")).ToList();

                foreach (var node in info_pelicula)
                {
                    var data = node.Descendants("b").Where(node => node.GetAttributeValue("class", "")
                    .Contains("info-item-label")).ToList().First().InnerText;


                    var contenido = node.Descendants("span").Where(node => node.GetAttributeValue("class", "")
                    .Contains("info-item-value")).ToList().First().InnerText;

                    if (data == "Genre:") { genero = contenido.Trim(); }

                    else if (data == "Release Date (Theaters):") { premier = contenido.Trim().Substring(0, 12); }

                    else if (data == "Runtime:") { duracion = contenido.Trim(); }
                }

                return new Pelicula
                {
                    Titulo = titulo,
                    plataformas = plataforma,
                    Url = url,
                    Img = img,
                    Calificacion_critica = calificacion_critica,
                    Calificacion_audiencia = calificacion_audiencia,
                    Sinopsis = sinopsis,
                    Genero = genero,
                    Premier = premier,
                    Duracion = duracion
                };
            }

            var response = call_url(url).Result;
            Pelicula pelicula = parse_html(response, url);
            return pelicula;
        }




        private bool TopPeliculasExists(int id)
        {
          return (_context.TopPeliculas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
