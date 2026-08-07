using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ManhwasWebApp.Data;
using ManhwasWebApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ManhwasWebApp.Controllers
{
    public class ManhwasController : Controller
    {
        private readonly ManhwasContext _context;

        public ManhwasController(ManhwasContext context)
        {
            _context = context;
        }

        // GET: Manhwas
        public async Task<IActionResult> Index()
        {
            var lista = await _context.VwDetalleManhwas
                .OrderByDescending(m => m.Calificacion)
                .ToListAsync();

            return View(lista);
        }

        // GET: Manhwas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manhwa = await _context.Manhwas
                .FirstOrDefaultAsync(m => m.IdManhwa == id);
            if (manhwa == null)
            {
                return NotFound();
            }

            return View(manhwa);
        }

        // GET: Manhwas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manhwas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ManhwaCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var connection = _context.Database.GetDbConnection();

            try
            {
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = "sp_RegistrarManhwaConTitulo";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@titulo", model.Titulo));
                command.Parameters.Add(new SqlParameter("@idioma", (object?)model.Idioma ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@novela", model.Novela == true ? "1" : "0"));
                command.Parameters.Add(new SqlParameter("@sinopsis", (object?)model.Sinopsis ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@url_portada", (object?)model.UrlPortada ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@calificacion", (object?)model.Calificacion ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@estado", (object?)model.Estado ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@anio_publicacion", (object?)model.AnioPublicacion ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@numero_capitulos", (object?)model.NumeroCapitulos ?? DBNull.Value));

                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError(string.Empty, "Error al registrar el manhwa: " + ex.Message);
                return View(model);
            }
            finally
            {
                await connection.CloseAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Manhwas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manhwa = await _context.Manhwas.FindAsync(id);
            if (manhwa == null)
            {
                return NotFound();
            }
            return View(manhwa);
        }

        // POST: Manhwas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdManhwa,Novela,Sinopsis,UrlPortada,Calificacion,Estado,AnioPublicacion,NumeroCapitulos,AnioFinalizacion")] Manhwa manhwa)
        {
            if (id != manhwa.IdManhwa)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(manhwa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManhwaExists(manhwa.IdManhwa))
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
            return View(manhwa);
        }

        // GET: Manhwas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manhwa = await _context.Manhwas
                .FirstOrDefaultAsync(m => m.IdManhwa == id);
            if (manhwa == null)
            {
                return NotFound();
            }

            return View(manhwa);
        }

        // POST: Manhwas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var manhwa = await _context.Manhwas.FindAsync(id);
            if (manhwa != null)
            {
                _context.Manhwas.Remove(manhwa);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ManhwaExists(int id)
        {
            return _context.Manhwas.Any(e => e.IdManhwa == id);
        }
    }
}
