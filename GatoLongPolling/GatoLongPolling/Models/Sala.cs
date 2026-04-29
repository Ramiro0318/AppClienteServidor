using System;
using System.Collections.Generic;
using System.Text;

namespace GatoLongPolling.Models
{
    public class Sala
    {
        public string? NombreJugador1{ get; set; } = null!;
        public string? IdJugador1 { get; set; } = null!;
        public string? NombreJugador2 { get; set; } = null!;
        public string? IdJugador2 { get; set; } = null!;

        public Gato Gato { get; set; } = new();

        public int Numero { get; set; }
    }
}
