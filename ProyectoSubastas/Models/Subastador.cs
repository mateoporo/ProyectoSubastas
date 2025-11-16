using ProyectoSubastas.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProyectoSubastas.Models
{
    public class Subastador
    {
        public int IdSubastador { get; set; }
        public string Nombre { get; set; }
        public string Mail { get; set; }

        public Subastador() { }

        public Subastador(int idSubastador, string nombre, string mail)
        {
            IdSubastador = idSubastador;
            Nombre = nombre;
            Mail = mail;
        }
    }
}
