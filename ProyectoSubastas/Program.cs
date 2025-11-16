using System;
using ProyectoSubastas.Controllers;
using ProyectoSubastas.Models;
using ProyectoSubastas.Views;

class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Login());
    }
}