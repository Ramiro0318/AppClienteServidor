using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Timers;

namespace PapaCalienteServidorUDP
{
    public class ServidorJuego
    {
        public List<JugadorInfo> Jugadores { get; set; } = new();

        int puerto = 15000;
        bool abierto = false;
        bool explotó = false;
        int tiempoGlobar, tiempoLocal;
        string combinacion;
        public UdpClient Server { get; set; }


        public void AbrirSala()
        {
            if (!abierto)
            {
                IPEndPoint ServerEndpoint = new(IPAddress.Any, puerto);
                Server = new(ServerEndpoint);
                abierto = true;
                Thread hilo = new(RecibirMensajes);
                hilo.IsBackground = true;
                hilo.Start();
            }
        }

        Random r = new();
        System.Windows.Forms.Timer timerGlobal;
        System.Windows.Forms.Timer timerLocal;
        public void IniciarJuego()
        {
            abierto = false;
            tiempoGlobar = r.Next(60, 121); // Entre 1 y 2 minutos

            timerGlobal = new System.Windows.Forms.Timer();
            timerGlobal.Interval = 1000;
            timerGlobal.Tick += TimerGlobal_Tick;

            timerGlobal.Start();
            EnviarPapa();

        }

        private void TimerGlobal_Tick(object? sender, EventArgs e)
        {
            if (tiempoLocal > 0)
            {
                tiempoGlobar--;
            }
            else
            {
                timerLocal?.Stop();
                ExplotarBomba();
            }
        }

        JugadorInfo? jugadorConPapa;
        public void EnviarPapa()
        {
            //Seleccionar un jugador con papa
            var jugador = Jugadores.Where(x => x.TienePapa == false).OrderBy(x => r.Next()).FirstOrDefault();

            if (jugadorConPapa != null)
            {
                jugadorConPapa.TienePapa = false;

            }
            if (jugador != null)
            {
                jugador?.TienePapa = true;
                jugadorConPapa = jugador;

            }
            //Eviar una combinacion de 5 letras


            combinacion = string.Concat(Enumerable.Range(0, 5).Select(x => (char)r.Next('A', 'Z' + 1)));

            //Enviar un comando a todos con la combinacion
            EnviarComando("Tiene_PAPA", jugadorConPapa.Usuario, combinacion, tiempoGlobar.ToString());

            //Timer local
            if (timerLocal == null)
            {
                timerLocal = new();
                timerLocal.Tick += TimerLocal_Tick;
            }
            timerLocal.Interval = 1000;
            timerLocal.Start();

        }

        private void TimerLocal_Tick(object? sender, EventArgs e)
        {
            if (tiempoLocal > 0)
            {
                tiempoGlobar--;
            }
            else
            {
                timerLocal?.Stop();
                ExplotarBomba();
            }
        }

        private void ExplotarBomba()
        {
            timerGlobal.Stop();
            timerLocal?.Stop();

            if (jugadorConPapa != null)
            {
                explotó = true;
                EnviarComando("EXPLOTA", jugadorConPapa.Usuario);
            }

        }

        public event Action<JugadorInfo>? JugadorAceptado;
        public void RecibirMensajes()
        {
            while (!explotó)
            {
                IPEndPoint clientEndPoint = new(IPAddress.None, 0);
                byte[] buffer = Server.Receive(ref clientEndPoint);

                string comando = Encoding.UTF8.GetString(buffer);

                string[] comandoSeparado = comando.Split('|');

                if (comandoSeparado[0] == "CONECTAR" && abierto && comandoSeparado.Length > 0
                    && !string.IsNullOrWhiteSpace(comandoSeparado[1]))
                {
                    //Creo el jugador que acepté
                    JugadorInfo jugador = new()
                    {
                        Nombre = comandoSeparado[1],
                        Ip = clientEndPoint.Address,
                        Puerto = clientEndPoint.Port,
                        TienePapa = false
                    };
                    //agregar a la lista
                    Jugadores.Add(jugador);

                    //Lanzar el evento a la UI
                    JugadorAceptado?.Invoke(jugador);

                    //Enviara comando Bienvenido a los clientes
                    var jugadores = string.Join(",", Jugadores.Select(x => x.Usuario));
                    EnviarComando("BIENVENIDO", jugadores);
                }
                if (comandoSeparado[0] == "COMBINACION" && !abierto && comandoSeparado.Length == 2)
                {
                    var combinacionUsuario = comandoSeparado[1];
                    if (combinacion == combinacionUsuario)
                    {
                        timerLocal?.Stop();
                        EnviarPapa();
                    }
                    else
                    {
                        ExplotarBomba();
                        break;
                    }
                }

            }

        }

        public void EnviarComando(string comando, string? parametro = null, string? parametro2 = null, string? parametro3 = null)
        {
            var mensaje = $"{comando}|{parametro}|{parametro2}|{parametro3}";

            byte[] buffer = Encoding.UTF8.GetBytes(mensaje);

            foreach (var cliente in Jugadores)
            {
                IPEndPoint destino = new IPEndPoint(cliente.Ip, cliente.Puerto);
                Server.Send(buffer, buffer.Length, destino);
            }
        }
    }
}
