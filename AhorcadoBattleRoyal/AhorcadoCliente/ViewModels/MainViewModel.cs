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
        private TipoVista _vistaActual = TipoVista.Conexion;

        public TipoVista VistaActual
        {
            get => _vistaActual;
            set { _vistaActual = value; OnPropertyChanged(); }
        }

        public ICommand IrASalaCommand { get; }
        public ICommand IrAJuegoCommand { get; }
        public ICommand VolverAConexionCommand { get; }
        public ICommand VerPartidaCommand { get; }
        public ICommand VolverCommand { get; }

        public MainViewModel()
        {
            IrASalaCommand = new RelayCommand(IrASala);
            IrAJuegoCommand = new RelayCommand(IrAJuego);
            VolverAConexionCommand = new RelayCommand(VolverAConexion);
            VerPartidaCommand = new RelayCommand(VerPartida);
            VolverCommand = new RelayCommand(Volver);
        }

        private void IrASala()
        {
            VistaActual = TipoVista.SalaEspera;
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