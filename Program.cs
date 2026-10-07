using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace D3_SemaforoPeluqueria
{
    internal class Program
    {
        static Semaphore cabina = new Semaphore(5, 5);
        public static int contador = 0;
        public static int tiempoAcumulado = 0;
        public static object bloqueo = new object();

        static void Main(string[] args)
        {
            Console.WriteLine("Simulación de una peluqueria con 5 peluqueros y 20 clientes.");

            for (int i = 1; i <= 20; i++)
            {
                Thread t = new Thread(() => Cliente("Cliente " + i));
                t.Start();
                int tiempoEspera = new Random().Next(2000, 8000);
                Thread.Sleep(tiempoEspera);
            }
        }

        static void Cliente(string nombreCliente)
        {
            Stopwatch sw = new Stopwatch();

            Console.WriteLine($"{nombreCliente} llega al centro de atención.\n");

            sw.Start();
            cabina.WaitOne();
            sw.Stop();

            int tiempoConsulta = new Random().Next(2000, 14000);

            Console.WriteLine($"------> {nombreCliente} está siendo atendido en una cabina.\n");

            Console.WriteLine($"{nombreCliente} tardó " + sw.ElapsedMilliseconds / 1000 + " segundos en ser atendido.\n");

            Thread.Sleep(tiempoConsulta);

            Console.WriteLine($"<------ {nombreCliente} terminó su atención y deja la cabina. Tardó " + tiempoConsulta /1000 + " segundos en salir.\n");

            lock (bloqueo)
            {
                tiempoAcumulado += tiempoConsulta;
                contador++;

                if (contador == 20)
                {
                    Console.WriteLine("Tiempo total (segundos): " + tiempoAcumulado / 1000);
                }
            }
            cabina.Release();
        }
    }
}
