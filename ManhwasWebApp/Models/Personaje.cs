using System;
using System.Collections.Generic;

namespace ManhwasWebApp.Models;

public partial class Personaje
{
    public int IdPersonaje { get; set; }

    public string Nombre { get; set; } = null!;

    public int? Edad { get; set; }

    public string? Ocupacion { get; set; }

    public int IdManhwa { get; set; }

    public virtual Manhwa IdManhwaNavigation { get; set; } = null!;
}
