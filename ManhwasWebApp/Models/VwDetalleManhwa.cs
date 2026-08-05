using System;
using System.Collections.Generic;

namespace ManhwasWebApp.Models;

public partial class VwDetalleManhwa
{
    public int IdManhwa { get; set; }

    public string? TituloPrincipal { get; set; }

    public string? Novela { get; set; }

    public string? Estado { get; set; }

    public decimal? Calificacion { get; set; }

    public int? NumeroCapitulos { get; set; }
    public string? UrlPortada { get; set; }

    public string Autores { get; set; } = null!;
}
