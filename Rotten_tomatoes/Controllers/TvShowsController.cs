using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rotten_tomatoes.Data;
using Rotten_tomatoes.Models;

namespace Rotten_tomatoes.Controllers
{
    public class TvShowsController : Controller
    {
        private readonly Rotten_tomatoesContext _context;

        public TvShowsController(Rotten_tomatoesContext context)
        {
            _context = context;
        }

        // GET: TvShows
        public async Task<IActionResult> Index()
        {
              return _context.TvShow != null ? 
                          View(await _context.TvShow.ToListAsync()) :
                          Problem("Entity set 'Rotten_tomatoesContext.TvShow'  is null.");
        }

        // GET: TvShows/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TvShow == null)
            {
                return NotFound();
            }

            var tvShow = await _context.TvShow
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tvShow == null)
            {
                return NotFound();
            }

            return View(tvShow);
        }

        // GET: TvShows/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TvShows/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string url)
        {
            var tvshow = scrape(url);

            _context.Add(tvshow);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }
        public static TvShow scrape(string url)
        {
            //Accediendo a url
            async Task<string> call_url(string fullUrl)
            {
                HttpClient client = new HttpClient();
                var response = await client.GetStringAsync(fullUrl);
                return response;
            }

            //parseando datos
            TvShow parse_html(string html, string url)
            {
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);


                string titulo = htmlDoc.DocumentNode.SelectNodes("td")
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

                return new TvShow
                {
                    Titulo = titulo,
                    Url = url,
                    Calificacion_critica = calificacion_critica,
                    Calificacion_audiencia = calificacion_audiencia,
                    Sinopsis = sinopsis,
                    Genero = genero,
                    Premier = premier
                };
            }

            var response = call_url(url).Result;
            TvShow tvshow = parse_html(response, url);
            return tvshow;
        }







        // GET: TvShows/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TvShow == null)
            {
                return NotFound();
            }

            var tvShow = await _context.TvShow
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tvShow == null)
            {
                return NotFound();
            }

            return View(tvShow);
        }

        // POST: TvShows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TvShow == null)
            {
                return Problem("Entity set 'Rotten_tomatoesContext.TvShow'  is null.");
            }
            var tvShow = await _context.TvShow.FindAsync(id);
            if (tvShow != null)
            {
                _context.TvShow.Remove(tvShow);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TvShowExists(int id)
        {
          return (_context.TvShow?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
