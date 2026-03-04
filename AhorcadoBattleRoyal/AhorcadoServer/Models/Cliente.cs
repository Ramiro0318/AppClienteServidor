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

        public TcpClient Conexion { get; set; }
        public string Nombre { get; set; } = null!;
        
    }
}
