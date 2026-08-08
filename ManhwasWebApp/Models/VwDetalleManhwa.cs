using System;
using System.Collections.Generic;

namespace ManhwasWebApp.Models;

public partial class VwDetalleManhwa
{
    public int IdManhwa { get; set; }

    public string? TituloPrincipal { get; set; }

    public bool? Novela { get; set; }

    public string? Sinopsis { get; set; }

    public string? Estado { get; set; }

    public decimal? Calificacion { get; set; }

    public int? NumeroCapitulos { get; set; }

    public int? AnioPublicacion { get; set; }

    public int? AnioFinalizacion { get; set; }

    public string? UrlPortada { get; set; }

    public string Autores { get; set; } = null!;

    public string Generos { get; set; } = null!;

    public string Etiquetas { get; set; } = null!;
}