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
    public class PersonajesController : Controller
    {
        private readonly ManhwasContext _context;

        public PersonajesController(ManhwasContext context)
        {
            _context = context;
        }

        // GET: Personajes
        public async Task<IActionResult> Index()
        {
            var manhwasContext = _context.Personajes.Include(p => p.IdManhwaNavigation);
            return View(await manhwasContext.ToListAsync());
        }

        // GET: Personajes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personaje = await _context.Personajes
                .Include(p => p.IdManhwaNavigation)
                .FirstOrDefaultAsync(m => m.IdPersonaje == id);
            if (personaje == null)
            {
                return NotFound();
            }

            return View(personaje);
        }

        // GET: Personajes/Create
        public IActionResult Create()
        {
            ViewData["IdManhwa"] = new SelectList(_context.Manhwas, "IdManhwa", "IdManhwa");
            return View();
        }

        // POST: Personajes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPersonaje,Nombre,Edad,Ocupacion,IdManhwa")] Personaje personaje)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personaje);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdManhwa"] = new SelectList(_context.Manhwas, "IdManhwa", "IdManhwa", personaje.IdManhwa);
            return View(personaje);
        }

        // GET: Personajes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personaje = await _context.Personajes.FindAsync(id);
            if (personaje == null)
            {
                return NotFound();
            }
            ViewData["IdManhwa"] = new SelectList(_context.Manhwas, "IdManhwa", "IdManhwa", personaje.IdManhwa);
            return View(personaje);
        }

        // POST: Personajes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPersonaje,Nombre,Edad,Ocupacion,IdManhwa")] Personaje personaje)
        {
            if (id != personaje.IdPersonaje)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personaje);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonajeExists(personaje.IdPersonaje))
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
            ViewData["IdManhwa"] = new SelectList(_context.Manhwas, "IdManhwa", "IdManhwa", personaje.IdManhwa);
            return View(personaje);
        }

        // GET: Personajes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personaje = await _context.Personajes
                .Include(p => p.IdManhwaNavigation)
                .FirstOrDefaultAsync(m => m.IdPersonaje == id);
            if (personaje == null)
            {
                return NotFound();
            }

            return View(personaje);
        }

        // POST: Personajes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var personaje = await _context.Personajes.FindAsync(id);
            if (personaje != null)
            {
                _context.Personajes.Remove(personaje);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonajeExists(int id)
        {
            return _context.Personajes.Any(e => e.IdPersonaje == id);
        }
    }
}
