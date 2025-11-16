using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ProyectoSubastas.Models
{
    public class Oferta
    {
        public int IdOferta { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaOferta { get; set; }
        public int IdSubasta { get; set; }
        public Subasta Subasta { get; set; }
        public int IdPostor { get; set; }
        public Postor Postor { get; set; }
        public Oferta() { }
    }
}
