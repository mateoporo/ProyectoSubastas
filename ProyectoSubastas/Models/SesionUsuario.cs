using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSubastas.Models
{
    public static class SesionUsuario
    {
        public static Postor PostorActual { get; set; }
        public static Subastador SubastadorActual { get; set; }

        public static bool EsPostor => PostorActual != null;
        public static bool EsSubastador => SubastadorActual != null;

        public static void Clear()
        {
            PostorActual = null;
            SubastadorActual = null;
        }
    }
}
