using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PapaCalienteServidorUDP
{
    public class ServidorJuego
    {
        public List<JugadorInfo> Jugadores { get; set; } = new();

        int puerto = 15000;
        bool abierto = false;
        bool explotó = false;
        public UdpClient Server { get; set; }


        public void AbrirSala()
        {
            if (!abierto)
            {
                IPEndPoint ServerEndpoint = new(IPAddress.Any, puerto);
                Server = new(ServerEndpoint);
                abierto = true;
                RecibirMensajes();
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
