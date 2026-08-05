using System;
using System.Collections.Generic;

namespace ManhwasWebApp.Models;

public partial class Etiquetum
{
    public int IdEtiqueta { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Manhwa> IdManhwas { get; set; } = new List<Manhwa>();
}
