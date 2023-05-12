using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rotten_tomatoes.Data;
using Rotten_tomatoes.Models;

namespace Rotten_tomatoes.Controllers
{
    public class TopTvShowsController : Controller
    {
        private readonly Rotten_tomatoesContext _context;

        public TopTvShowsController(Rotten_tomatoesContext context)
        {
            _context = context;
        }

        // GET: TopTvShows
        public async Task<IActionResult> Index()
        {
              return _context.TopTvShow != null ? 
                          View(await _context.TopTvShow.ToListAsync()) :
                          Problem("Entity set 'Rotten_tomatoesContext.TopTvShow'  is null.");
        }

        // GET: TopTvShows/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TopTvShow == null)
            {
                return NotFound();
            }

            var topTvShow = await _context.TopTvShow
                .FirstOrDefaultAsync(m => m.Id == id);
            if (topTvShow == null)
            {
                return NotFound();
            }

            return View(topTvShow);
        }

        // GET: TopTvShows/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TopTvShows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Url,Titulo,Img,Calificacion_critica,Calificacion_audiencia,plataformas,Sinopsis,Genero,Premier")] TopTvShow topTvShow)
        {
            if (ModelState.IsValid)
            {
                _context.Add(topTvShow);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(topTvShow);
        }

        // GET: TopTvShows/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TopTvShow == null)
            {
                return NotFound();
            }

            var topTvShow = await _context.TopTvShow.FindAsync(id);
            if (topTvShow == null)
            {
                return NotFound();
            }
            return View(topTvShow);
        }

        // POST: TopTvShows/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Url,Titulo,Img,Calificacion_critica,Calificacion_audiencia,plataformas,Sinopsis,Genero,Premier")] TopTvShow topTvShow)
        {
            if (id != topTvShow.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(topTvShow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TopTvShowExists(topTvShow.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(topTvShow);
        }

        // GET: TopTvShows/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TopTvShow == null)
            {
                return NotFound();
            }

            var topTvShow = await _context.TopTvShow
                .FirstOrDefaultAsync(m => m.Id == id);
            if (topTvShow == null)
            {
                return NotFound();
            }

            return View(topTvShow);
        }

        // POST: TopTvShows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TopTvShow == null)
            {
                return Problem("Entity set 'Rotten_tomatoesContext.TopTvShow'  is null.");
            }
            var topTvShow = await _context.TopTvShow.FindAsync(id);
            if (topTvShow != null)
            {
                _context.TopTvShow.Remove(topTvShow);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TopTvShowExists(int id)
        {
          return (_context.TopTvShow?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
