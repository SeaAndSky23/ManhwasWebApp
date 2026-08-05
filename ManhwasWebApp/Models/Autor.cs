using System;
using System.Collections.Generic;

namespace ManhwasWebApp.Models;

public partial class Autor
{
    public int IdAutor { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<ManhwaAutor> ManhwaAutors { get; set; } = new List<ManhwaAutor>();
}
