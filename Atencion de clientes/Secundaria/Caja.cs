//librerias
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;



namespace Secundaria
{
    //Clase caja
    public class Caja
    {
        //atributos
        private static Random random;
        private Queue<string> clientesALaEspera;
        private string nombreCaja;

        // propiedades
        public delegate void DelegadoClienteAtendido(Caja caja, string cliente);
       private DelegadoClienteAtendido delegadoClienteAtendido;
        

        //constructor

        //instanciar la cola de clientes a la espera con un tontador y el nombre de la caja
        public int CantidadDeClientesALaEspera
        {
            get
            {
                return clientesALaEspera.Count;
            }
        }


        //nombre caja retorna nombre de la caja asignada ( caja 01 o 02)
        public string NombreCaja
        {
            get
            {
                return nombreCaja;
            }
        }

        static Caja()
        {
            random = new Random();
        }

        public Caja(string nombreCaja, DelegadoClienteAtendido delegadoClienteAtendido)
        {
            //clientes a la espera en nueva cola 
            clientesALaEspera = new Queue<string>();
            this.delegadoClienteAtendido = delegadoClienteAtendido;
            this.nombreCaja = nombreCaja;
        }

        internal void AgregarCliente(string cliente)
        {
            //agrega al cliente en la cola a cliente
            clientesALaEspera.Enqueue(cliente);
        }

        //ATENCION A CLIENTES//

        //inicia la atencion
        internal Task IniciarAtencion()
        {
            return Task.Run(AtenderClientes);
        }


        //tarea de atencion
        private void AtenderClientes()
        {
            //se crea le -do while- para que la tarea siga hasta que el programa se cierre 
            do
            {
                if(clientesALaEspera.Any()) // si clientes a la espera todavia tiene clientes entra y sigue con la atencion.
                {
                    string cliente = clientesALaEspera.Dequeue(); // manda el cliente a la espera a atender
                    delegadoClienteAtendido.Invoke(this, cliente); // Manda al cliente en la caja como cliente atendido
                    Thread.Sleep(random.Next(1000, 5000)); //timer para suspender el hilo entre 1 y 5 segundos
                }
            }
            while (true);
        }
    }
}