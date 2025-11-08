using Microsoft.Win32;
using ServiciosEducativosComunitarios.Model;
using ServiciosEducativosComunitarios.Repositories;
using ServiciosEducativosComunitarios.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ServiciosEducativosComunitarios.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        // almacena el usuario
        private string _username = "";

        // almacena la contraseña de forma segura
        private SecureString? _password;

        // Guarda el mensaje de error si el login falla
        private string? _errorMessage;

        // controla si la ventana de login está visible
        private bool _isViewVisible = true;

        private IUserRepository _userRepository;

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public SecureString? Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string? ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public bool IsViewVisible
        {
            get { return _isViewVisible; }
            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(_isViewVisible));
            }
        }

        // Se ejecuta cuando el usuario hace click en login
        public ICommand LoginCommand { get; }

        // Mostrar/ocultar la contraseña
        public ICommand? ShowPasswordCommand { get; }

        public LoginViewModel()
        {
            _userRepository = new UserRepository();
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
        }

        private bool CanExecuteLoginCommand(object? obj)
        {
            return !(string.IsNullOrWhiteSpace(Username) || Username.Length < 3 || Password == null || Password.Length < 3);
        }

        private void ExecuteLoginCommand(object? obj)
        {
            var isValidUser = _userRepository.AuthenticateUser(new System.Net.NetworkCredential(Username, Password));

            if (isValidUser)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username), null);
                IsViewVisible = false;

                // Crear nueva ventana
                AppView appView = new AppView();
                // Asignarla como ventana principal
                Application.Current.MainWindow = appView;
                // Mostrarla
                appView.Show();
                // Cerrar la anterior (login)
                foreach (Window window in Application.Current.Windows)
                {
                    if (window != appView)
                    {
                        window.Close();
                        break;
                    }
                }
            }
            else
            {
                ErrorMessage = "* Invalid username or password";
            }
        }
    }
}
