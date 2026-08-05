using System;
using System.Collections.Generic;

namespace ManhwasWebApp.Models;

public partial class Genero
{
    public int IdGenero { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Manhwa> IdManhwas { get; set; } = new List<Manhwa>();
}
