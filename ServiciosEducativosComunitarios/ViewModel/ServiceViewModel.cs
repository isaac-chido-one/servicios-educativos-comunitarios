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

        public ICommand AddCommand
        {
            get
            {
                return new ViewModelCommand(AddExecute, AddCanExecute);
            }
        }

        private void AddExecute(object service)
        {
            serviceRepository.Add(Service);
            MessageBox.Show("Servicio agregado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private bool AddCanExecute(object user)
        {
            return true;
        }
    }
}
