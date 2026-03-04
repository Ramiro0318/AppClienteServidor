using AhorcadoServer.Models;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AhorcadoServer.Services
{
    public class AhorcadoService
    {
        TcpListener? Servidor;

        public List<Cliente> Clientes { get; set; } = [];

        int puertoEscucha = 7777;
        private string? fraseAdivinar, fraseOculta;
        public int Errores { get; set; }
        public Cliente? Turno { get; set; }
        public int Ronda { get; set; }
        public string? FaseOculta { get; set; }
        public char[]? LetrasDisponibles { get; set; } = [];

        bool salaAbierta = false;

        public event Action<string>? LogEnviado;
        public void AbrirSala()
        {
            if (!salaAbierta)
            {
                salaAbierta = true;


                LogEnviado?.Invoke("Servidor iniciado");

                Thread hilo = new(RecibirClientes);
                hilo.IsBackground = true;
                hilo.Start();


            }
        }

        private void RecibirClientes()
        {
            IPEndPoint ipServer = new(IPAddress.Any, puertoEscucha);
            Servidor = new(ipServer);

            while (salaAbierta)
            {
                try
                {
                    var clienteNuevo = Servidor.AcceptTcpClient();
                    var stram = clienteNuevo.ToString();

                    byte[] buffer = new byte[clienteNuevo.Available];
                    var json = Encoding.UTF8.GetString(buffer);

                    var conectarCommand = JsonSerializer.Deserialize<ConectarComand>(json);

                    if (conectarCommand != null)
                    {
                        var nombre = conectarCommand.Nombre;
                        lock (Clientes)
                        {
                            if (Clientes.Any(x => x.Nombre == nombre))
                            {
                                //lo rechazo
                                var rechazarCommand = new RechazarComando
                                {
                                    Comando = Orden.Rechazar
                                };
                                EnviarComando(rechazarCommand, clienteNuevo);
                                clienteNuevo.Close();
                            }
                            else
                            {
                                //Lo agrego a la lista
                                var cliente = new Cliente
                                {
                                    Conexion = clienteNuevo,
                                    Nombre = nombre ?? "",
                                };

                                Clientes.Add(cliente);

                                var bienvenido = new BienvenidoComando
                                {
                                    Comando = Orden.Bienvenido,
                                    Nombres = Clientes.Select(x => x.Nombre).ToList(),
                                };

                                EnviarComando(bienvenido, clienteNuevo);

                                Thread hilo = new Thread(EscucharCliente);
                                hilo.IsBackground = true;
                                hilo.Start();
                            }
                        }
                    }

                }
                catch
                {

                }
            }
        }

        public void CerrarSala()
        {
            salaAbierta = false;
            Servidor?.Stop();

            LogEnviado?.Invoke("La sala ha sid cerrada");
            IniciarRonda("");
        }

        public void IniciarRonda(string frase)
        {
            Errores = 0;
            fraseAdivinar = frase.ToUpper();
            Ronda++;
            LetrasDisponibles = "ABCDEFGHIJKLMOPQRSTUVWXYZ".ToCharArray();

            fraseOculta = string.Join("", frase.Select(letra => char.IsLetter(letra) ? '_' : letra));

            //Seleccionar jugador turno
            Random r = new Random();
            Turno = Clientes[r.Next(0, Clientes.Count)];

            var comando = new TurnoComando
            {
                Comando = Orden.CambiarTurno,
                JugadorTurno = Turno.Nombre,
                LetrasDisponibles = LetrasDisponibles,
                NumErrores = Errores,
                Palabra = fraseOculta
            };

            EnviarTodos(comando);

        }

        private void EscucharCliente(object? cliente)
        {
            if (cliente != null)
            {
                TcpClient client = (TcpClient)cliente;

                try
                {
                    while (client.Connected)
                    {
                        if (client.Available > 0)
                        {
                            var stream = client.GetStream();
                            var buffer = new byte[client.Available];
                            stream.ReadExactly(buffer, 0, buffer.Length);

                            var json = Encoding.UTF8.GetString(buffer);
                            var comando = JsonSerializer.Deserialize<ResponderComando>(json);

                            if (comando != null)
                            {
                                LogEnviado?.Invoke("letra recibida" + comando.Letra);
                                ProcesarLetra(comando.Letra);
                            }

                        }
                        else
                        {
                            Thread.Sleep(100);
                        }
                    }
                }
                catch { }
            }
        }

        private void ProcesarLetra(string? letra)
        {
            if (letra != null && fraseAdivinar != null)
            {
                LetrasDisponibles = LetrasDisponibles.Except(letra).ToArray();

                if (fraseAdivinar.Contains(letra))
                {
                    //Acertó
                }
                else
                {
                    //Se equivocó
                }
                if (Errores > 6)
                {

                }
                else
                {

                }
            }

        }

        private void EnviarTodos(Comandos comandos)
        {
            lock (Clientes)
            {
                foreach (var cliente in Clientes)
                {
                    if (cliente != null)
                    {
                        EnviarComando(comandos, cliente.Conexion);

                    }
                }
            }
        }

        private void EnviarComando(Comandos Comando, TcpClient cliente)
        {
            var stream = cliente.GetStream();
            var json = JsonSerializer.Serialize(stream);
            var buffer = Encoding.UTF8.GetBytes(json);
            stream.Write(buffer, 0, buffer.Length);
        }
    }
}
