using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rotten_tomatoes.Data;
using Rotten_tomatoes.Models;
using HtmlAgilityPack;
using System.Drawing;

namespace Rotten_tomatoes.Controllers
{
    public class PeliculasController : Controller
    {
        private readonly Rotten_tomatoesContext _context;

        public PeliculasController(Rotten_tomatoesContext context)
        {
            _context = context;
        }

        // GET: Peliculas
        public async Task<IActionResult> Index()
        {
              return _context.Pelicula != null ? 
                          View(await _context.Pelicula.ToListAsync()) :
                          Problem("Entity set 'Rotten_tomatoesContext.Pelicula'  is null.");
        }

        // GET: Peliculas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pelicula == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Pelicula
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        // GET: Peliculas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Peliculas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string url)
        {
            var pelicula = scrape(url);

            _context.Add(pelicula);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public static Pelicula scrape(string url)
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


                string titulo  = htmlDoc.DocumentNode.Descendants("img")
                .Where(node => node.ParentNode.GetAttributeValue("class", "").Contains("thumbnail")).
                ToList().First().GetAttributeValue("alt", " ").Remove(0, 18);

                string calificacion_critica = htmlDoc.DocumentNode.Descendants("score-board")
                .Where(node => node.GetAttributeValue("id", "").Contains("scoreboard"))
                .ToList().First().GetAttributeValue("tomatometerscore", "");

                string calificacion_audiencia = htmlDoc.DocumentNode.Descendants("score-board")
                .Where(node => node.GetAttributeValue("id", "").Contains("scoreboard"))
                .ToList().First().GetAttributeValue("audiencescore", "");

                string sinopsis = htmlDoc.DocumentNode.Descendants("p")
                .Where(node => node.GetAttributeValue("data-qa", "").
                Contains("movie-info-synopsis")).ToList().First().InnerText.Trim();

                var info_central_ = htmlDoc.DocumentNode.Descendants("li")
                .Where(node => node.GetAttributeValue("class", "").Contains("info-item")).ToList();

                string genero = "Accion";
                string duracion = "2h";
                string premier = "10/07/23";
                

                //foreach (var node in info_central_)
                //{
                //    var info = node.Descendants("b").Where(node => node.GetAttributeValue("class", "")
                //    .Contains("info-item-label")).ToList().First().InnerText;


                //    var valor = node.Descendants("span").Where(node => node.GetAttributeValue("class", "")
                //    .Contains("info-item-value")).ToList().First().InnerText;

                //     if (info == "Genre:") { genero = valor.Trim(); }

                //     else if (info == "Release Date (Theaters):") { premier = valor.Trim().Substring(0, 12); }

                //     else if (info == "Runtime:") { duracion = valor.Trim(); }
                //}

                return new Pelicula
                {
                    Titulo = titulo,
                    Url = url,
                    Calificacion_critica = calificacion_critica,
                    Calificacion_audiencia = calificacion_audiencia,
                    Sinopsis =sinopsis,
                    Genero = genero,
                    Premier = premier,
                    Duracion = duracion
                };
            }

            var response = call_url(url).Result;
            Pelicula pelicula = parse_html(response, url);
            return pelicula;
        }
      





        // GET: Peliculas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pelicula == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Pelicula
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        // POST: Peliculas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pelicula == null)
            {
                return Problem("Entity set 'Rotten_tomatoesContext.Pelicula'  is null.");
            }
            var pelicula = await _context.Pelicula.FindAsync(id);
            if (pelicula != null)
            {
                _context.Pelicula.Remove(pelicula);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeliculaExists(int id)
        {
          return (_context.Pelicula?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
