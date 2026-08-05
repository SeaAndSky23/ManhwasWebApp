using System;
using System.Collections.Generic;

namespace ManhwasWebApp.Models;

public partial class Manhwa
{
    public int IdManhwa { get; set; }

    public bool? Novela { get; set; }

    public string? Sinopsis { get; set; }

    public string? UrlPortada { get; set; }

    public decimal? Calificacion { get; set; }

    public string? Estado { get; set; }

    public int? AnioPublicacion { get; set; }

    public int? NumeroCapitulos { get; set; }

    public int? AnioFinalizacion { get; set; }

    public virtual ICollection<ManhwaAutor> ManhwaAutors { get; set; } = new List<ManhwaAutor>();

    public virtual ICollection<Personaje> Personajes { get; set; } = new List<Personaje>();

    public virtual ICollection<Titulo> Titulos { get; set; } = new List<Titulo>();

    public virtual ICollection<Etiquetum> IdEtiqueta { get; set; } = new List<Etiquetum>();

    public virtual ICollection<Genero> IdGeneros { get; set; } = new List<Genero>();
}
