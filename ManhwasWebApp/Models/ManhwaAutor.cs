using System;
using System.Collections.Generic;

namespace ManhwasWebApp.Models;

public partial class ManhwaAutor
{
    public int IdManhwa { get; set; }

    public int IdAutor { get; set; }

    public string Rol { get; set; } = null!;

    public virtual Autor IdAutorNavigation { get; set; } = null!;

    public virtual Manhwa IdManhwaNavigation { get; set; } = null!;
}
