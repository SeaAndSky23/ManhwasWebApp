using System.ComponentModel.DataAnnotations;
namespace ManhwasWebApp.Models
{
    public class ManhwaEditViewModel
    {
        public int IdManhwa { get; set; }

        public bool? Novela { get; set; }

        public string? Sinopsis { get; set; }

        [StringLength(255)]
        public string? UrlPortada { get; set; }

        [Range(0, 10)]
        public decimal? Calificacion { get; set; }

        [StringLength(50)]
        public string? Estado { get; set; }

        public int? AnioPublicacion { get; set; }

        public int? NumeroCapitulos { get; set; }

        public int? AnioFinalizacion { get; set; }

        public List<int> SelectedGeneros { get; set; } = new();

        public List<int> SelectedEtiquetas { get; set; } = new();
    }
}