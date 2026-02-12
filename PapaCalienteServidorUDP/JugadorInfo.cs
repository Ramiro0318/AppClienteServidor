using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PapaCalienteServidorUDP
{
    public class JugadorInfo
    {
        public string Nombre { get; set; } = null!;
        public IPAddress Ip { get; set; } = null!;
        public int Puerto { get; set; }
        public bool TienePapa { get; set; }
        public string Usuario => $"{Nombre}@{Ip}:{Puerto}";
    }
}
