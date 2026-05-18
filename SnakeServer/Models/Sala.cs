using System.Drawing;

namespace SnakeServer.Models
{
    public class Sala
    {
        public Tablero Tablero { get; set; } = new();

        public string? IdJugador1 { get; set; }
        public string? IdJugador2 { get; set; }

        public string? NombreJugador1 { get; set; }
        public string? NombreJugador2 { get; set; }

    }
}
