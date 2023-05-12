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
                Contains("series-info-description")).ToList().First().InnerText.Trim();

                var plataformas = htmlDoc.DocumentNode.Descendants("where-to-watch-meta")
                .Where(node => node.GetAttributeValue("data-qa", "").Contains("affiliate-item"))
                .ToList();

                string total = "";
                foreach (var node in plataformas)
                {
                    total += node.GetAttributeValue("affiliate", "-") + "";

                }

                string genero = "";
                string premier = "";

                var info_tvshow = htmlDoc.DocumentNode.Descendants("li")
               .Where(node => node.GetAttributeValue("class", "").Contains("info-item")).ToList();

                foreach (var node in info_tvshow)
                {
                    if (!(node == info_tvshow[0]))
                    {
                        premier = "";
                        
                    }
                    else
                    {
                        genero = node.Descendants("span").Where(node => node.GetAttributeValue("class", "")
                        .Contains("info-item-value")).ToList().First().InnerText.Trim();
                    }
                }

                return new TvShow
                {
                    Titulo = titulo,
                    Img = img,
                    plataformas = total,
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
