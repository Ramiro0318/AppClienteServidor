using System;
using System.Collections.Generic;
using System.Text;

namespace GatoLongPolling.DTOs
{
    public class TableroDTO
    {
        public string? MensajeSuperior { get; set; }
        public string? MensajeInferior { get; set; }
        public string[]? Tablero { get; set; }
        public bool Terminado { get; set; }
        public string? IdTurno { get; set; }
    }
}
