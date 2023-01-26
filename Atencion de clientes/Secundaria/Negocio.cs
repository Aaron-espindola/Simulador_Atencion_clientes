//librerias
using System;
using NameGenerator.Generators; //paquete nuget de name generator para generar nombres aleatorios
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Secundaria
{
    public class Negocio // Clase de Negocio
    {
        private static RealNameGenerator realNameGenerator; //creacion del atributo name generator. static
        private ConcurrentQueue<string> clientes; //atributo para las queue(colas)
        private List<Caja> cajas; //atributo para las cajas



        static Negocio()
        {
            realNameGenerator = new RealNameGenerator();

        }

        public Negocio(List<Caja> cajas)// instancia de cajas y de clientes
        {
            this.cajas = cajas;
            clientes = new ConcurrentQueue<string>();
        }


        //Metodo publico para comenzar la atencion
        public List<Task> ComenzarAtencion()
        {
            List<Task> hilos = new List<Task>();

            hilos.AddRange(AbrirCajas()); //se llama al metodo -abrir cajas- para signar cada caja y lo aloja en un hilo
            hilos.Add(Task.Run(GenerarClientes)); // Se llama al metodo -generar clientes- y lo aloja en otro hilo
            hilos.Add(Task.Run(AsignarCajas)); // se llama al metodo -Asignar cajas- y tambien se aloja en otro hilo

            return hilos;
        }


        //Abrir cajas
        private List<Task> AbrirCajas()
        {
            List<Task> hilos = new List<Task>();

            foreach(Caja caja in cajas) //for each para recorrer los hilos para poder iniciar abrir las cajas y comenzar la atencion
            {
                hilos.Add(caja.IniciarAtencion());
            }

            return hilos;
        }


        //Metodo para generar clientes
        private void GenerarClientes()
        {
            do //se instancia un do while para la generacion aleatoria de clientes
            {
                clientes.Enqueue(realNameGenerator.Generate());
                Thread.Sleep(1000); //Se agrega un cliente nuevo a la cola cada  segundo
            }
            while (true);
        }


        //Metodo de asignar cajas
        private void AsignarCajas()
        {
            do //se instancia un do while para que el programa siga asignando clientes a las cajas hasta que el programa se cierre
            {
                Caja caja = cajas.OrderBy(c => c.CantidadDeClientesALaEspera).First(); // se ordenan las cajas de forma acsendiente (caja con mas elementos) luego con el metodo first se toma el primer elemento (caja con menos clientes)
                clientes.TryDequeue(out string cliente); // recuperamos el cliente con TryDequeue
                if (!string.IsNullOrWhiteSpace(cliente)) // Si cliente retorna un string que no es nulo entra al if
                {
                    caja.AgregarCliente(cliente); // se agrega el nuevo cliente a la caja con el metodo agregar cliente
                }
            }
            while (true);
        }
    }
}
