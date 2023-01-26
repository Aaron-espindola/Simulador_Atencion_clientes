using Secundaria; //importa el proyecto "secundaria"
//librerias
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

class Exam_Hilos 
{
    static void Main()
    {
        //se instancia el delegado
        Caja.DelegadoClienteAtendido delegadoClienteAtendido = (caja, cliente) =>
        {
            //Creamos un console write para que muetre el horario, el hilo, las cajas y la cantidad de gente en cada caja (las filas).
            Console.WriteLine($"{DateTime.Now:HH:MM:ss} - hilo {Task.CurrentId} - {caja.NombreCaja} - Atendiendo al cliente {cliente}. Quedan {caja.CantidadDeClientesALaEspera} clientes la fila.");

        };
         // Se instancian las cajas y se les pasa el delegado
        Caja caja00 = new Caja("Caja 01", delegadoClienteAtendido);
        Caja caja01 = new Caja("Caja 02", delegadoClienteAtendido);

        //lista de cajas // se agregan las cajas creadas a la lista
        List<Caja> cajas = new List<Caja>()
        {
            caja00,caja01
        };

        // se crea un -new negocio-  y se pasan la lista con las cajas
        Negocio negocio = new Negocio(cajas);
        //console Write para la asignacion de cajas,
        Console.WriteLine("Asignando cajas espere por favor...");

        //Lista con las taks y los hilos que se usan. se llama al metodo -comenzar atencion-
        List<Task> hilos = negocio.ComenzarAtencion();
        Task.WaitAll(hilos.ToArray()); //el task esperando a los hilos // se utiliza el metodo WaitAll para que el programa no se cierre
    }
}