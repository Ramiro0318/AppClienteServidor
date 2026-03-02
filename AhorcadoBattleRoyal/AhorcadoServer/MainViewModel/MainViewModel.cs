using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AhorcadoServer.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private bool _modalNuevaRondaVisible;

        public bool ModalNuevaRondaVisible
        {
            get => _modalNuevaRondaVisible;
            set { _modalNuevaRondaVisible = value; OnPropertyChanged(); }
        }

        public ICommand IniciarServidorCommand { get; }
        public ICommand MostrarModalNuevaRondaCommand { get; }
        public ICommand AbrirSalaCommand { get; }
        public ICommand CancelarNuevaRondaCommand { get; }
        public ICommand IniciarNuevaRondaCommand { get; }

        public MainViewModel()
        {
            IniciarServidorCommand = new RelayCommand(IniciarServidor);
            MostrarModalNuevaRondaCommand = new RelayCommand(MostrarModalNuevaRonda);
            AbrirSalaCommand = new RelayCommand(AbrirSala);
            CancelarNuevaRondaCommand = new RelayCommand(CancelarNuevaRonda);
            IniciarNuevaRondaCommand = new RelayCommand(IniciarNuevaRonda);
        }

        private void IniciarServidor(object? parameter = null)
        {
        }

        private void MostrarModalNuevaRonda(object? parameter = null)
        {
            ModalNuevaRondaVisible = true;
        }

        private void AbrirSala(object? parameter = null)
        {
        }

        private void CancelarNuevaRonda(object? parameter = null)
        {
            ModalNuevaRondaVisible = false;
        }

        private void IniciarNuevaRonda(object? parameter)
        {
            string frase = parameter as string ?? "";
            ModalNuevaRondaVisible = false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}