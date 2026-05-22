using System.Drawing;

namespace SnakeServer.Models
{
    public enum Direccion { Izquierda, Derecha, Arriba, Abajo };

    public class Tablero
    {
        public Point Manzana { get; set; }
        public int Ancho { get; set; } = 20;
        public int Largo { get; set; } = 20;

        public List<Point> Serpiente1 { set; get; } = [];
        public List<Point> Serpiente2 { set; get; } = [];

        public bool Terminado { get; set; }
        public string? Ganador { get; set; }
        public int Puntos1 { set; get; }
        public int Puntos2 { set; get; }

        public Direccion Direccion1 { get; set; }
        public Direccion Direccion2 { get; set; }
    }
}
