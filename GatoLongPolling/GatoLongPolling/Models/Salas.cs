using System;
using System.Collections.Generic;
using System.Text;

namespace GatoLongPolling.Models
{
    public class Salas
    {
        public List<Sala> SalasDisponibles { get; set; } = new();

        public Sala? SolicitarSala(string id) 
        {
            var existentes = SalasDisponibles.FirstOrDefault(x => x.IdJugador1 == id || x.IdJugador2 == id);
                return existentes;
        }

        public Sala SolicitarSala(string nombre, string id) 
        {
            //Verificar si ya tiene sala
            var existentes = SalasDisponibles.FirstOrDefault(x => x.IdJugador1 == id || x.IdJugador2 == id);
            if (existentes != null) 
            {
                return existentes;
            }

            var salaAbierta = SalasDisponibles.FirstOrDefault(x => x.IdJugador1 == null || x.IdJugador2 == null);

            if (salaAbierta == null) 
            {   //No has abiertas
                Sala nueva = new();
                nueva.Numero = SalasDisponibles.Count == 0 ? 1 : SalasDisponibles.Max(x => x.Numero) + 1;
                nueva.IdJugador1 = id;
                nueva.NombreJugador1 = nombre;
                SalasDisponibles.Add(nueva);
                return nueva;
            }
            else
            {
                salaAbierta.IdJugador2 = id;
                salaAbierta.NombreJugador2 = nombre;
                return salaAbierta;
            }
        }
    }
}
