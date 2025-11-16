using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ProyectoSubastas.Models
{
    public class Subasta
    {
        public int IdSubasta { get; set; }

        [Required]
        [StringLength(200)]
        public string Articulo { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "La puja inicial debe ser mayor a cero.")]
        public decimal PujaInicial { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "La puja de aumento debe ser mayor a cero.")]
        public decimal PujaAumento { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        public bool Activa => DateTime.Now >= FechaInicio && DateTime.Now <= FechaFin;

        public int IdSubastador { get; set; }
        public Subastador Subastador { get; set; }

        // relación con ofertas
        public List<Oferta> Ofertas { get; set; } = new();

        // lista de postores participando
        public List<Postor> Participantes { get; set; } = new();

        public Subasta() { }

        public IList<ValidationResult> Validate()
        {
            var ctx = new ValidationContext(this);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(this, ctx, results, true);
            return results;
        }

        public Oferta ObtenerUltimaOferta()
        {
            return Ofertas.OrderByDescending(o => o.FechaOferta).FirstOrDefault();
        }
    }
}
