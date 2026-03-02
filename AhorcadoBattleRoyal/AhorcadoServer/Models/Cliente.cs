using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AhorcadoServer.Models
{
    public class Cliente
    {
        public TcpListener? Servidor;
        public List<Cliente> Clientes { get; set; } = [];

        int puertoEscucha = 7777;

        private string? fraseAdivinar;
        public int Errores;
        public Cliente Turno { get; set; }
        public int Ronda { get; set; }

        public string? FraseOculta { get; set; }
        public List<string> LetrasDisponibles { get; set; }
    }
}
