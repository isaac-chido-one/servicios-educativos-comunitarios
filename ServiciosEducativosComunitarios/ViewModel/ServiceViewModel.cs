using ServiciosEducativosComunitarios.Model;
using ServiciosEducativosComunitarios.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ServiciosEducativosComunitarios.ViewModel
{
    public class ServiceViewModel : ViewModelBase
    {
        private readonly RepositoryBase? repositoryBase;

        private ObservableCollection<ServiceModel>? _services;

        private ServiceModel _service;

        private IServiceRepository serviceRepository;

        public ServiceModel Service { 
            get { return _service; } 
            set {
                _service = value;
                OnPropertyChanged(nameof(Service));
            }
        }

        public ObservableCollection<ServiceModel> Services
        {
            get { return _services; }
            set {
                if (_services != value)
                {
                    _services = value;
                    OnPropertyChanged(nameof(Services));
                }
            }
        }

        public ServiceViewModel()
        {
            serviceRepository = new ServiceRepository();
            _service = new ServiceModel();
        }

        private bool validate()
        {
            if (string.IsNullOrEmpty(Service.Code))
            {
                MessageBox.Show("Ingresa una clave", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (Service.LocalityId == 0)
            {
                MessageBox.Show("Selecciona una localidad", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (Service.Program == 0)
            {
                MessageBox.Show("Selecciona un programa", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (Service.Period == 0)
            {
                MessageBox.Show("Selecciona un ciclo escolar", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (Service.Status == 0)
            {
                MessageBox.Show("Selecciona un status", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        public ICommand AddCommand
        {
            get
            {
                return new ViewModelCommand(AddExecute, AddCanExecute);
            }
        }

        private void AddExecute(object service)
        {
            if (this.validate())
            {
                serviceRepository.Add(Service);
                MessageBox.Show("Servicio agregado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private bool AddCanExecute(object service)
        {
            return true;
        }

        public ICommand UpdateCommand
        {
            get
            {
                return new ViewModelCommand(UpdateExecute, UpdateCanExecute);
            }
        }

        private void UpdateExecute(object service)
        {
            if (this.validate())
            {
                serviceRepository.Update(Service);
                MessageBox.Show("Servicio actualizado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private bool UpdateCanExecute(object service)
        {
            return true;
        }

    }
}
