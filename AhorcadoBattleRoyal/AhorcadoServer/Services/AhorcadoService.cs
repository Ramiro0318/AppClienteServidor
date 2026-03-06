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
        public event Action? RondaRequerida;
        public event Action? JuegoFinalizado;
        public event Action<Cliente>? ClienteConectado;
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
                                ClienteConectado?.Invoke(cliente);
                                LogEnviado?.Invoke($"{cliente.Nombre} se ha conectado");

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

        }

        private void EnviarEstado(Cliente? cliente)
        {
            var comando = new TurnoComando
            {
                Comando = Orden.CambiarTurno,
                JugadorTurno = Turno?.Nombre,
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
                    LogEnviado?.Invoke($" Jugador {Turno?.Nombre} se acertó la letra {letra}");
                    //Acertó
                    string nuevaFraseOculta = "";

                    for (int i = 0; i < fraseAdivinar.Length; i++)
                    {
                        if (fraseAdivinar[i] == char.Parse(letra))
                        {
                            nuevaFraseOculta += letra;
                        }
                        else
                        {
                            nuevaFraseOculta += fraseOculta?[i];
                        }
                    }
                    fraseOculta = nuevaFraseOculta;

                    EnviarEstado(null);

                    Thread.Sleep(3000);
                    if (fraseOculta == fraseAdivinar)
                    {
                        var CambiarRondaComando = new CambiarRondaComando
                        {
                            Comando = Orden.CambiarRonda,
                        };

                        EnviarTodos(CambiarRondaComando);
                        LogEnviado?.Invoke("Se acabó la ronda, adivinaron la frase");

                        RondaRequerida?.Invoke();
                    }
                    else
                    {
                        CambiarTurno();
                        LogEnviado?.Invoke($"El jugador {Turno?.Nombre} tiene el turno");
                        EnviarEstado(Turno);
                    }
                }
                else
                {
                    //Se equivocó
                    Errores++;
                    LogEnviado?.Invoke("El jugador se equivocó con la letra: " + letra);

                    var turno = Turno;
                    Turno = null;

                    EnviarEstado(null);
                    //Pausa para observar ultima jugada
                    Thread.Sleep(3000);

                    if (Errores == 6 && turno != null)
                    {
                        var comando = new ExpulsarComando
                        {
                            Comando = Orden.Expulsar,
                            Jugador = turno.Nombre,
                            Palabra = fraseAdivinar
                        };
                        EnviarTodos(comando);
                        LogEnviado?.Invoke($"El jugador {turno.Nombre} ha sido expulsado");

                        turno.Conexion.Close();
                        lock (Clientes)
                        {
                            Clientes.Remove(turno);
                        }

                        //Si solo queda un cliente este ganó
                        if (Clientes.Count > 1)
                        {
                            RondaRequerida?.Invoke();
                        }
                        else
                        {
                            var clienteGanador = Clientes[0];
                            var ganador = new GanadorComando
                            {
                                Comando = Orden.Ganar,
                                Jugador = Clientes[0]?.Nombre
                            };
                            Clientes.Clear();
                            clienteGanador.Conexion.Close();

                        }


                        RondaRequerida?.Invoke();
                    }
                    else
                    {
                        Turno = turno;
                        CambiarTurno();
                        LogEnviado?.Invoke($"El jugador {turno} tiene el turno");
                        EnviarEstado(turno);
                    }
                }
            }

        }

        private void CambiarTurno()
        {
            if (Turno != null)
            {
                var indice = Clientes.IndexOf(Turno);
                Turno = indice + 1 == Clientes.Count ? Clientes[0] : Clientes[indice + 1];
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
