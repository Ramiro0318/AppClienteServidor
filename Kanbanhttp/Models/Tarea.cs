using System;
using System.Collections.Generic;
using System.Text;

namespace Kanbanhttp.Models
{
    public enum Estados { Pendiente, EnProceso, Hecho, Finalizado}
    public class Tarea
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = null!;
        public DateTime? Fecha { get; set; }
        public string Usuario { get; set; } = null!;
        public Estados Estado { get; set; }
    }
}
