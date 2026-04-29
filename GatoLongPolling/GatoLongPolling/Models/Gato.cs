using System;
using System.Collections.Generic;
using System.Text;

namespace GatoLongPolling.Models
{
    public class Gato
    {
        public string[] Tablero { get; private set; }
        public string? Turno { get; private set; }  
        public string? Ganador { get; private set; }
        public bool Terminado { get; private set; }
        public int Version { get; private set; }

        public Gato()
        {
            Tablero = new string[9];
            Turno = "X";
            Ganador = null;
            Terminado = false;
            Version = 0;
        }

        public bool HacerMovimiento(string simbolo, int posicion)
        {
            if (Terminado) return false;
            if (simbolo != Turno) return false;
            if (posicion < 0 || posicion > 8) return false;
            if (!string.IsNullOrEmpty(Tablero[posicion])) return false;

            Tablero[posicion] = simbolo;
            Version++;

            if (HayGanador(simbolo))
            {
                Ganador = simbolo;
                Terminado = true;
            }
            else if (EstaLleno())
            {
                Ganador = "empate";
                Terminado = true;
            }
            else
            {
                Turno = (Turno == "X") ? "O" : "X";
            }

            return true;
        }

        private bool HayGanador(string simbolo)
        {
            int[][] combinaciones = new int[][]
            {
            new[] {0,1,2}, new[] {3,4,5}, new[] {6,7,8},  
            new[] {0,3,6}, new[] {1,4,7}, new[] {2,5,8},  
            new[] {0,4,8}, new[] {2,4,6}                  
            };

            foreach (var combo in combinaciones)
            {
                if (Tablero[combo[0]] == simbolo &&
                    Tablero[combo[1]] == simbolo &&
                    Tablero[combo[2]] == simbolo)
                {
                    return true;
                }
            }
            return false;
        }

        private bool EstaLleno()
        {
            foreach (var pos in Tablero)
            {
                if (string.IsNullOrEmpty(pos)) return false;
            }
            return true;
        }

    }
}
