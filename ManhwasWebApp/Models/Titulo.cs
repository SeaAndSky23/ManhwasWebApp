using System;
using System.Collections.Generic;

namespace ManhwasWebApp.Models;

public partial class Titulo
{
    public int IdTitulo { get; set; }

    public string Titulo1 { get; set; } = null!;

    public string? Idioma { get; set; }

    public int IdManhwa { get; set; }

    public virtual Manhwa IdManhwaNavigation { get; set; } = null!;
}
