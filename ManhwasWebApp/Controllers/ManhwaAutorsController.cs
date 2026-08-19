using Microsoft.Data.SqlClient;
using System.Data;
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
    public class ManhwaAutorsController : Controller
    {
        private readonly ManhwasContext _context;

        public ManhwaAutorsController(ManhwasContext context)
        {
            _context = context;
        }

        // GET: ManhwaAutors
        public async Task<IActionResult> Index()
        {
            var manhwasContext = _context.ManhwaAutors.Include(m => m.IdAutorNavigation).Include(m => m.IdManhwaNavigation);
            return View(await manhwasContext.ToListAsync());
        }

        // GET: ManhwaAutors/Details/5/3
        public async Task<IActionResult> Details(int? idManhwa, int? idAutor)
        {
            if (idManhwa == null || idAutor == null)
            {
                return NotFound();
            }

            var manhwaAutor = await _context.ManhwaAutors
                .Include(m => m.IdAutorNavigation)
                .Include(m => m.IdManhwaNavigation)
                .FirstOrDefaultAsync(m => m.IdManhwa == idManhwa && m.IdAutor == idAutor);
            if (manhwaAutor == null)
            {
                return NotFound();
            }

            return View(manhwaAutor);
        }

        // GET: ManhwaAutors/Create
        public IActionResult Create()
        {
            ViewData["IdAutor"] = new SelectList(_context.Autors, "IdAutor", "Nombre");
            ViewData["IdManhwa"] = new SelectList(
                _context.VwDetalleManhwas.OrderBy(m => m.TituloPrincipal),
                "IdManhwa", "TituloPrincipal");
            return View();
        }

        // POST: ManhwaAutors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdManhwa,IdAutor,Rol")] ManhwaAutor manhwaAutor)
        {
            if (!ModelState.IsValid)
            {
                ViewData["IdAutor"] = new SelectList(_context.Autors, "IdAutor", "Nombre", manhwaAutor.IdAutor);
                ViewData["IdManhwa"] = new SelectList(
                    _context.VwDetalleManhwas.OrderBy(m => m.TituloPrincipal),
                    "IdManhwa", "TituloPrincipal", manhwaAutor.IdManhwa);
                return View(manhwaAutor);
            }

            var connection = _context.Database.GetDbConnection();

            try
            {
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = "sp_AsociarAutorAManhwa";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@id_manhwa", manhwaAutor.IdManhwa));
                command.Parameters.Add(new SqlParameter("@id_autor", manhwaAutor.IdAutor));
                command.Parameters.Add(new SqlParameter("@rol", manhwaAutor.Rol));

                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError(string.Empty, "Error al asociar el autor: " + ex.Message);
                ViewData["IdAutor"] = new SelectList(_context.Autors, "IdAutor", "Nombre", manhwaAutor.IdAutor);
                ViewData["IdManhwa"] = new SelectList(
                    _context.VwDetalleManhwas.OrderBy(m => m.TituloPrincipal),
                    "IdManhwa", "TituloPrincipal", manhwaAutor.IdManhwa);
                return View(manhwaAutor);
            }
            finally
            {
                await connection.CloseAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        /// GET: ManhwaAutors/Edit/5/3
        public async Task<IActionResult> Edit(int? idManhwa, int? idAutor)
        {
            if (idManhwa == null || idAutor == null)
            {
                return NotFound();
            }

            var manhwaAutor = await _context.ManhwaAutors
                .FirstOrDefaultAsync(m => m.IdManhwa == idManhwa && m.IdAutor == idAutor);
            if (manhwaAutor == null)
            {
                return NotFound();
            }
            ViewData["IdAutor"] = new SelectList(_context.Autors, "IdAutor", "Nombre", manhwaAutor.IdAutor);
            ViewData["IdManhwa"] = new SelectList(
                _context.VwDetalleManhwas.OrderBy(m => m.TituloPrincipal),
                "IdManhwa", "TituloPrincipal", manhwaAutor.IdManhwa);
            return View(manhwaAutor);
        }

        // POST: ManhwaAutors/Edit/5/3
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int idManhwa, int idAutor, [Bind("IdManhwa,IdAutor,Rol")] ManhwaAutor manhwaAutor)
        {
            if (idManhwa != manhwaAutor.IdManhwa || idAutor != manhwaAutor.IdAutor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(manhwaAutor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManhwaAutorExists(manhwaAutor.IdManhwa, manhwaAutor.IdAutor))
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
            ViewData["IdAutor"] = new SelectList(_context.Autors, "IdAutor", "IdAutor", manhwaAutor.IdAutor);
            ViewData["IdManhwa"] = new SelectList(_context.Manhwas, "IdManhwa", "IdManhwa", manhwaAutor.IdManhwa);
            return View(manhwaAutor);
        }


        // GET: ManhwaAutors/Delete/5/3
        public async Task<IActionResult> Delete(int? idManhwa, int? idAutor)
        {
            if (idManhwa == null || idAutor == null)
            {
                return NotFound();
            }

            var manhwaAutor = await _context.ManhwaAutors
                .Include(m => m.IdAutorNavigation)
                .Include(m => m.IdManhwaNavigation)
                .FirstOrDefaultAsync(m => m.IdManhwa == idManhwa && m.IdAutor == idAutor);
            if (manhwaAutor == null)
            {
                return NotFound();
            }

            return View(manhwaAutor);
        }

        // POST: ManhwaAutors/Delete/5/3
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int idManhwa, int idAutor)
        {
            var manhwaAutor = await _context.ManhwaAutors
                .FirstOrDefaultAsync(m => m.IdManhwa == idManhwa && m.IdAutor == idAutor);
            if (manhwaAutor != null)
            {
                _context.ManhwaAutors.Remove(manhwaAutor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ManhwaAutorExists(int idManhwa, int idAutor)
        {
            return _context.ManhwaAutors.Any(e => e.IdManhwa == idManhwa && e.IdAutor == idAutor);
        }
    }
}
