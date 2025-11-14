using System;
using ProyectoSubastas.Controllers;
using ProyectoSubastas.Models;
using ProyectoSubastas.Views;

class Program
{
    [STAThread]
    static void Main()
    {
        // TEST SUBASTADOR
        /*
        var controller = new SubastadorController();
        controller.CrearSubastador("Pepe Gomez", "pepe@mail.com");
        controller.CrearSubastador("Juan Gutierrez", "juan@mail.com");

        var lista = controller.ListarSubastadores();
        Console.WriteLine("📋 Listado de subastadores:");
        foreach (var p in lista)
            Console.WriteLine($"{p.IdSubastador} | {p.Nombre} | {p.Mail}");
        */

        // TEST POSTOR
        /*
        var controller = new PostorController();
        controller.CrearPostor("Felipa Gomez", "felipa@mail.com");
        controller.CrearPostor("Fede Gutierrez", "fede@mail.com");

        var lista = controller.ListarPostores();
        Console.WriteLine("📋 Listado de postores:");
        foreach (var p in lista)
            Console.WriteLine($"{p.IdPostor} | {p.Nombre} | {p.Mail}");
        */

        // Inicialización clásica para evitar dependencias de ApplicationConfiguration (plantilla VS)
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Login());

    }
}