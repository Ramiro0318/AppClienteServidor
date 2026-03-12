using AhorcadoCliente.Models;
using AhorcadoCliente.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AhorcadoCliente.ViewModels
{
    public enum TipoVista
    {
        Conexion,
        SalaEspera,
        Juego,
        Eliminado,
        Ganador
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        ClienteService clienteService = new();
        public ObservableCollection<string> Jugadores { get; set; } = new();

        public string? Nombre { get; set; }
        public string? DireccionIP { get; set; } = "127.0.0.1";
        public string? Mensaje { get; set; }
        public bool EsMiTurno { get { return turno.JugadorTurno == Nombre; } set; }

        private TurnoComando turno;

        public TurnoComando Turno
        {
            get { return turno; }
            set { turno = value; OnPropertyChanged(); }
        }


        private TipoVista _vistaActual = TipoVista.Conexion;

        public TipoVista VistaActual
        {
            get => _vistaActual;
            set
            {
                _vistaActual = value; OnPropertyChanged();
                OnPropertyChanged(nameof(EsMiTurno));
            }
        }

        public ICommand IrASalaCommand { get; }
        public ICommand IrAJuegoCommand { get; }
        public ICommand VolverAConexionCommand { get; }
        public ICommand VerPartidaCommand { get; }
        public ICommand VolverCommand { get; }

        Dispatcher dispatceher;

        public MainViewModel()
        {

            dispatceher = new();
            IrASalaCommand = new RelayCommand(IrASala);
            IrAJuegoCommand = new RelayCommand(IrAJuego);
            VolverAConexionCommand = new RelayCommand(VolverAConexion);
            VerPartidaCommand = new RelayCommand(VerPartida);
            VolverCommand = new RelayCommand(Volver);

            clienteService.JugadorConectado += clienteService_JugadorConectado;
            clienteService.JugadorRechazado += clienteService_JugadorRechazado;
            clienteService.TurnoCambiado += clienteService_TurnoCambiado;
        }

        private void clienteService_TurnoCambiado()
        {
            dispatceher.BeginInvoke(() =>
            {

                if (VistaActual == TipoVista.SalaEspera)
                {
                    VistaActual = TipoVista.Juego;
                }
                turno = obj;
            });
        }
        private void clienteService_JugadorRechazado()
        {
            dispatceher.BeginInvoke(() =>
            {
                Mensaje = "El nombre seleccionado ya está siendo utilizado";
                OnPropertyChanged(nameof(Mensaje));
            });
        }

        private void clienteService_JugadorConectado()
        {
            Jugadores.Clear();
            obj.ForEach(J => Jugadores.add(j));
            dispatceher.BeginInvoke(() =>
            {
                Mensaje = "";
                if (VistaActual == TipoVista.Conexion)
                {
                    VistaActual = TipoVista.SalaEspera;
                }
                OnPropertyChanged(nameof(Mensaje));
            });
        }

        private void IrASala()
        {
            Mensaje = string.Empty;
            if (string.isNullOrWhiteSpace)
            {
                Mensaje = "Escribe el nombre del jugador";
                OnPropertyChanged(nameof(Mensaje));
                return;

            }
            if (!IPAddress.TryParse(DireccionIP, out IPAddress? ip))
            {
                Mensaje = "Escriba una direccion IP correcta";
                OnPropertyChanged(nameof(Mensaje));
                return;
            }

            clienteService.Conectar(DireccionIP, Nombre);

        }

        private void IrAJuego()
        {
            VistaActual = TipoVista.Juego;
        }

        private void VolverAConexion()
        {
            VistaActual = TipoVista.Conexion;
        }

        private void VerPartida()
        {
            VistaActual = TipoVista.Juego;
        }

        private void Volver()
        {
            VistaActual = TipoVista.Conexion;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;

        public RelayCommand(Action execute)
        {
            _execute = execute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _execute();
        }
    }
}