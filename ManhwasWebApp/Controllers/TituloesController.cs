using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ManhwasWebApp.Data;
using ManhwasWebApp.Models;

namespace ManhwasWebApp.Controllers
{
    public class TituloesController : Controller
    {
        private readonly ManhwasContext _context;

        public TituloesController(ManhwasContext context)
        {
            _context = context;
        }

        // GET: Tituloes
        public async Task<IActionResult> Index()
        {
            var manhwasContext = _context.Titulos.Include(t => t.IdManhwaNavigation);
            return View(await manhwasContext.ToListAsync());
        }

        // GET: Tituloes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var titulo = await _context.Titulos
                .Include(t => t.IdManhwaNavigation)
                .FirstOrDefaultAsync(m => m.IdTitulo == id);
            if (titulo == null)
            {
                return NotFound();
            }

            return View(titulo);
        }

        // GET: Tituloes/Create
        public IActionResult Create()
        {
            ViewData["IdManhwa"] = new SelectList(_context.Manhwas, "IdManhwa", "IdManhwa");
            return View();
        }

        // POST: Tituloes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTitulo,Titulo1,Idioma,IdManhwa")] Titulo titulo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(titulo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdManhwa"] = new SelectList(_context.Manhwas, "IdManhwa", "IdManhwa", titulo.IdManhwa);
            return View(titulo);
        }

        // GET: Tituloes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var titulo = await _context.Titulos.FindAsync(id);
            if (titulo == null)
            {
                return NotFound();
            }
            ViewData["IdManhwa"] = new SelectList(_context.Manhwas, "IdManhwa", "IdManhwa", titulo.IdManhwa);
            return View(titulo);
        }

        // POST: Tituloes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTitulo,Titulo1,Idioma,IdManhwa")] Titulo titulo)
        {
            if (id != titulo.IdTitulo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(titulo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TituloExists(titulo.IdTitulo))
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
            ViewData["IdManhwa"] = new SelectList(_context.Manhwas, "IdManhwa", "IdManhwa", titulo.IdManhwa);
            return View(titulo);
        }

        // GET: Tituloes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var titulo = await _context.Titulos
                .Include(t => t.IdManhwaNavigation)
                .FirstOrDefaultAsync(m => m.IdTitulo == id);
            if (titulo == null)
            {
                return NotFound();
            }

            return View(titulo);
        }

        // POST: Tituloes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var titulo = await _context.Titulos.FindAsync(id);
            if (titulo != null)
            {
                _context.Titulos.Remove(titulo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TituloExists(int id)
        {
            return _context.Titulos.Any(e => e.IdTitulo == id);
        }
    }
}
