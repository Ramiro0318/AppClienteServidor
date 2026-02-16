using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PapaCalienteClienteUDP
{
    public class ClienteJuego
    {
        public string? Nick { get; set; }
        UdpClient Cliente = new();
        int puertoServer = 15000;
        bool explotó = false;

        public void Conectar(IPAddress ipServer, string usuario)
        {
            //Sanitizar el usuaario, no permito el pipe ni el colon

            usuario = usuario.Replace("|", "").Replace(",", "");
            IPEndPoint remoto = new IPEndPoint(ipServer, puertoServer);

            var comando = $"CONECTAR|{usuario}";
            byte[] buffer = Encoding.UTF8.GetBytes(comando);

            Cliente.Send(buffer, buffer.Length, remoto);

            Thread hilo = new(RecibirComandos);
            hilo.IsBackground = true;
            hilo.Start();
        }

        public event Action<string[]>? UsuariosRecibidos;
        public void RecibirComandos()
        {
            while (!explotó)
            {
                IPEndPoint remoto = new(IPAddress.None, 0);
                byte[] buffer = Cliente.Receive(ref remoto);

                var comando = Encoding.UTF8.GetString(buffer);

                var comandoSeparado = comando.Split('|');

                if (comandoSeparado[0] == "BIENVENIDO")
                {
                    var usuarios = comandoSeparado[1].Split(",");
                    UsuariosRecibidos?.Invoke(usuarios);
                }

            }

        }


    }
}
