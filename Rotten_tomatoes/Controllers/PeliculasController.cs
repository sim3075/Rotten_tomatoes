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


                string titulo  = htmlDoc.DocumentNode.SelectNodes("td")
                    .Where(node => node.GetAttributeValue("aria-label", "")
                    .Contains("Name"))
                    .ToList()[0].InnerText;

                string calificacion_critica = htmlDoc.DocumentNode.SelectNodes("td")
                    .Where(node => node.GetAttributeValue("aria-label", "")
                    .Contains("Price (Intraday)"))
                    .ToList()[0].FirstChild.InnerText;

                string calificacion_audiencia = htmlDoc.DocumentNode.SelectNodes("td")
                    .Where(node => node.GetAttributeValue("aria-label", "")
                    .Contains("Change"))
                    .ToList()[0].FirstChild.FirstChild.InnerText;

                string sinopsis = htmlDoc.DocumentNode.SelectNodes("td")
                    .Where(node => node.GetAttributeValue("aria-label", "")
                    .Contains("% Change"))
                    .ToList()[0].FirstChild.FirstChild.InnerText;

                string genero = htmlDoc.DocumentNode.SelectNodes("td")
                    .Where(node => node.GetAttributeValue("aria-label", "")
                    .Contains("% Change"))
                    .ToList()[0].FirstChild.FirstChild.InnerText;

                string premier = htmlDoc.DocumentNode.SelectNodes("td")
                    .Where(node => node.GetAttributeValue("aria-label", "")
                    .Contains("% Change"))
                    .ToList()[0].FirstChild.FirstChild.InnerText;

                string duracion = htmlDoc.DocumentNode.SelectNodes("td")
                    .Where(node => node.GetAttributeValue("aria-label", "")
                    .Contains("% Change"))
                    .ToList()[0].FirstChild.FirstChild.InnerText;

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
