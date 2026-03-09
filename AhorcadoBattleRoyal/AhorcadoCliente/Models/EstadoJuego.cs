using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhorcadoCliente.Models
{
    public class EstadoJuego
    {
        public string? Palabra { get; set; }
        public string? JugadorTurno { get; set; } = null!;
        public int NumErrores { get; set; }
        public char[] LetrasIniciales { get; set; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        public char[] LetrasDisponibles { get; set; } = [];
        public int Ronda { get; set; }
    }
}
