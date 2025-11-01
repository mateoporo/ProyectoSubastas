using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProyectoSubastas.Models
{
    public class Postor
    {
        public int IdPostor { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El mail es obligatorio.")]
        [EmailAddress(ErrorMessage = "El mail no tiene un formato válido.")]
        public string Mail { get; set; }

        public Postor() { }

        public Postor(int idPostor, string nombre, string mail)
        {
            IdPostor = idPostor;
            Nombre = nombre;
            Mail = mail;
        }

        public IList<ValidationResult> Validate()
        {
            var ctx = new ValidationContext(this);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(this, ctx, results, validateAllProperties: true);
            return results;
        }
    }
}

