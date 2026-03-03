using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhorcadoServer.Models
{
    public enum Orden { Conectar, Bienvenido, Rechazar, CambiarTurno, Responder, Expulsar, Ganar, CambiarRonda }
    public class Comandos
    {
        public Orden Comando { get; set; }
    }

    public class ConectarComand : Comandos
    {
        public string Nombre { get; set; } = null!;
    }

    public class BienvenidoComando : Comandos
    {
        public List<string> Nombres { get; set; } = [];
    }

    public class RechazarComando : Comandos { }

    public class TurnoComando : Comandos
    {
        public string? Palabra { get; set; }
        public string? JugadorTurno { get; set; } = null!;
        public int NumErrores { get; set; }
        public List<string> LetrasDisponibles { get; set; } = [];
    }

    public class ResponderComando : Comandos
    {
        public string? Letra { get; set; }
    }

    public class ExpulsarComando : Comandos
    {
        public string? Jugador { get; set; }
        public string? Palabra { get; set; }
    }

    public class GanadorComando : Comandos
    {
        public string? Jugador { get; set; }
    }

    public class CambiarRondaComand : Comandos { }
}
