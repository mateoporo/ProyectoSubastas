using ProyectoSubastas.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProyectoSubastas.Models
{
    public class Subastador
    {
        public int IdSubastador { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El mail es obligatorio.")]
        [EmailAddress(ErrorMessage = "El mail no tiene un formato válido.")]
        public string Mail { get; set; }

        public Subastador() { }

        public Subastador(int idSubastador, string nombre, string mail)
        {
            IdSubastador = idSubastador;
            Nombre = nombre;
            Mail = mail;
        }

        /// <summary>
        /// Valida las propiedades usando DataAnnotations. Devuelve lista de errores vacía si ok.
        /// </summary>
        public IList<ValidationResult> Validate()
        {
            var ctx = new ValidationContext(this);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(this, ctx, results, validateAllProperties: true);
            return results;
        }
    }
}
