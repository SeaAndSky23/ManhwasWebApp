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
    public class EtiquetumsController : Controller
    {
        private readonly ManhwasContext _context;

        public EtiquetumsController(ManhwasContext context)
        {
            _context = context;
        }

        // GET: Etiquetums
        public async Task<IActionResult> Index()
        {
            return View(await _context.Etiqueta.ToListAsync());
        }

        // GET: Etiquetums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var etiquetum = await _context.Etiqueta
                .FirstOrDefaultAsync(m => m.IdEtiqueta == id);
            if (etiquetum == null)
            {
                return NotFound();
            }

            return View(etiquetum);
        }

        // GET: Etiquetums/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Etiquetums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEtiqueta,Nombre")] Etiquetum etiquetum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(etiquetum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(etiquetum);
        }

        // GET: Etiquetums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var etiquetum = await _context.Etiqueta.FindAsync(id);
            if (etiquetum == null)
            {
                return NotFound();
            }
            return View(etiquetum);
        }

        // POST: Etiquetums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEtiqueta,Nombre")] Etiquetum etiquetum)
        {
            if (id != etiquetum.IdEtiqueta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(etiquetum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EtiquetumExists(etiquetum.IdEtiqueta))
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
            return View(etiquetum);
        }

        // GET: Etiquetums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var etiquetum = await _context.Etiqueta
                .FirstOrDefaultAsync(m => m.IdEtiqueta == id);
            if (etiquetum == null)
            {
                return NotFound();
            }

            return View(etiquetum);
        }

        // POST: Etiquetums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var etiquetum = await _context.Etiqueta.FindAsync(id);
            if (etiquetum != null)
            {
                _context.Etiqueta.Remove(etiquetum);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EtiquetumExists(int id)
        {
            return _context.Etiqueta.Any(e => e.IdEtiqueta == id);
        }
    }
}
