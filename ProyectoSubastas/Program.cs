using System;
using ProyectoSubastas.Controllers;
using ProyectoSubastas.Models;

class Program
{
    static void Main()
    {
        //using var controller = new ProyectoSubastas.Controllers.SubastadorController("bd_subastas.db");

        // TEST SUBASTADOR
        // Crear
        //var creado = controller.Crear("Pepe Gutierrez", "pepe@gmail.com");
        //Console.WriteLine($"Creado Id: {creado.IdSubastador}, Nombre: {creado.Nombre}");

        // Listar
        /*
        var todos = controller.Listar();
        Console.WriteLine("Listado:");
        foreach (var s in todos) Console.WriteLine($"{s.IdSubastador} | {s.Nombre} | {s.Mail}");

        // Obtener
        var uno = controller.Obtener(creado.IdSubastador);
        Console.WriteLine($"Obtenido: {uno.Nombre}");

        // Actualizar
        uno.Nombre = "Pepe G.";
        controller.Actualizar(uno);
        Console.WriteLine("Actualizado.");

        // Eliminar
        controller.Eliminar(uno.IdSubastador);
        Console.WriteLine("Eliminado.");
        */

        // TEST POSTOR
        /*
        using var controller = new PostorController();
        controller.Crear("Ana Gómez", "ana@mail.com");
        controller.Crear("Luis Torres", "luis@mail.com");

        var lista = controller.Listar();
        Console.WriteLine("📋 Listado de postores:");
        foreach (var p in lista)
            Console.WriteLine($"{p.IdPostor} | {p.Nombre} | {p.Mail}");
        */
    }
}