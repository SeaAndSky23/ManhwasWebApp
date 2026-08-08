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

            var manhwa = await _context.VwDetalleManhwas
                .FirstOrDefaultAsync(m => m.IdManhwa == id);
            if (manhwa == null)
            {
                return NotFound();
            }

            return View(manhwa);
        }

        // GET: Manhwas/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.AllGeneros = await _context.Generos.OrderBy(g => g.Nombre).ToListAsync();
            ViewBag.AllEtiquetas = await _context.Etiqueta.OrderBy(e => e.Nombre).ToListAsync();
            return View();
        }


        // POST: Manhwas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ManhwaCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AllGeneros = await _context.Generos.OrderBy(g => g.Nombre).ToListAsync();
                ViewBag.AllEtiquetas = await _context.Etiqueta.OrderBy(e => e.Nombre).ToListAsync();
                return View(model);
            }

            var connection = _context.Database.GetDbConnection();
            int nuevoManhwaId;

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

                var result = await command.ExecuteScalarAsync();
                nuevoManhwaId = Convert.ToInt32(result);
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError(string.Empty, "Error al registrar el manhwa: " + ex.Message);
                ViewBag.AllGeneros = await _context.Generos.OrderBy(g => g.Nombre).ToListAsync();
                ViewBag.AllEtiquetas = await _context.Etiqueta.OrderBy(e => e.Nombre).ToListAsync();
                return View(model);
            }
            finally
            {
                await connection.CloseAsync();
            }

            // Asociar géneros y etiquetas seleccionados
            var manhwa = await _context.Manhwas
                .Include(m => m.IdGeneros)
                .Include(m => m.IdEtiqueta)
                .FirstOrDefaultAsync(m => m.IdManhwa == nuevoManhwaId);

            if (manhwa != null)
            {
                foreach (var gid in model.SelectedGeneros)
                {
                    var genero = await _context.Generos.FindAsync(gid);
                    if (genero != null) manhwa.IdGeneros.Add(genero);
                }

                foreach (var eid in model.SelectedEtiquetas)
                {
                    var etiqueta = await _context.Etiqueta.FindAsync(eid);
                    if (etiqueta != null) manhwa.IdEtiqueta.Add(etiqueta);
                }

                await _context.SaveChangesAsync();
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

            var manhwa = await _context.Manhwas
                .Include(m => m.IdGeneros)
                .Include(m => m.IdEtiqueta)
                .FirstOrDefaultAsync(m => m.IdManhwa == id);

            if (manhwa == null)
            {
                return NotFound();
            }

            var viewModel = new ManhwaEditViewModel
            {
                IdManhwa = manhwa.IdManhwa,
                Novela = manhwa.Novela,
                Sinopsis = manhwa.Sinopsis,
                UrlPortada = manhwa.UrlPortada,
                Calificacion = manhwa.Calificacion,
                Estado = manhwa.Estado,
                AnioPublicacion = manhwa.AnioPublicacion,
                NumeroCapitulos = manhwa.NumeroCapitulos,
                AnioFinalizacion = manhwa.AnioFinalizacion,
                SelectedGeneros = manhwa.IdGeneros.Select(g => g.IdGenero).ToList(),
                SelectedEtiquetas = manhwa.IdEtiqueta.Select(e => e.IdEtiqueta).ToList()
            };

            ViewBag.AllGeneros = await _context.Generos.OrderBy(g => g.Nombre).ToListAsync();
            ViewBag.AllEtiquetas = await _context.Etiqueta.OrderBy(e => e.Nombre).ToListAsync();

            return View(viewModel);
        }

        // POST: Manhwas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ManhwaEditViewModel model)
        {
            if (id != model.IdManhwa)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.AllGeneros = await _context.Generos.OrderBy(g => g.Nombre).ToListAsync();
                ViewBag.AllEtiquetas = await _context.Etiqueta.OrderBy(e => e.Nombre).ToListAsync();
                return View(model);
            }

            var manhwa = await _context.Manhwas
                .Include(m => m.IdGeneros)
                .Include(m => m.IdEtiqueta)
                .FirstOrDefaultAsync(m => m.IdManhwa == id);

            if (manhwa == null)
            {
                return NotFound();
            }

            manhwa.Novela = model.Novela;
            manhwa.Sinopsis = model.Sinopsis;
            manhwa.UrlPortada = model.UrlPortada;
            manhwa.Calificacion = model.Calificacion;
            manhwa.Estado = model.Estado;
            manhwa.AnioPublicacion = model.AnioPublicacion;
            manhwa.NumeroCapitulos = model.NumeroCapitulos;
            manhwa.AnioFinalizacion = model.AnioFinalizacion;

            // Sincronizar géneros
            var generosActuales = manhwa.IdGeneros.Select(g => g.IdGenero).ToList();
            var generosAQuitar = manhwa.IdGeneros.Where(g => !model.SelectedGeneros.Contains(g.IdGenero)).ToList();
            foreach (var g in generosAQuitar) manhwa.IdGeneros.Remove(g);

            var generosAAgregar = model.SelectedGeneros.Except(generosActuales).ToList();
            foreach (var gid in generosAAgregar)
            {
                var genero = await _context.Generos.FindAsync(gid);
                if (genero != null) manhwa.IdGeneros.Add(genero);
            }

            // Sincronizar etiquetas
            var etiquetasActuales = manhwa.IdEtiqueta.Select(e => e.IdEtiqueta).ToList();
            var etiquetasAQuitar = manhwa.IdEtiqueta.Where(e => !model.SelectedEtiquetas.Contains(e.IdEtiqueta)).ToList();
            foreach (var e in etiquetasAQuitar) manhwa.IdEtiqueta.Remove(e);

            var etiquetasAAgregar = model.SelectedEtiquetas.Except(etiquetasActuales).ToList();
            foreach (var eid in etiquetasAAgregar)
            {
                var etiqueta = await _context.Etiqueta.FindAsync(eid);
                if (etiqueta != null) manhwa.IdEtiqueta.Add(etiqueta);
            }

            try
            {
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

        // GET: Manhwas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manhwa = await _context.VwDetalleManhwas
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
