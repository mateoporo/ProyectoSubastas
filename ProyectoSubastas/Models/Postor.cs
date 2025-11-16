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
        public string Nombre { get; set; }
        public string Mail { get; set; }
        public Postor() { }
        public Postor(int idPostor, string nombre, string mail)
        {
            IdPostor = idPostor;
            Nombre = nombre;
            Mail = mail;
        }
    }
}

