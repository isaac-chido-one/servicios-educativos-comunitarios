using Microsoft.IdentityModel.Logging;
using Microsoft.VisualBasic.ApplicationServices;
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
    public class LocalityViewModel : ViewModelBase
    {
        private readonly RepositoryBase? repositoryBase;

        private ObservableCollection<LocalityModel>? _localities;

        private LocalityModel _locality;

        private ILocalityRepository localityRepository;

        public LocalityModel Locality
        {
            get { return _locality; }
            set
            {
                _locality = value;
                OnPropertyChanged(nameof(Locality));
            }
        }

        public ObservableCollection<LocalityModel> Localities
        {
            get { return _localities; }
            set
            {
                if (_localities != value)
                {
                    _localities = value;
                    OnPropertyChanged(nameof(Localities));
                }
            }
        }

        public LocalityViewModel()
        {
            localityRepository = new LocalityRepository();
            _locality = new LocalityModel();
        }

        public ICommand AddCommand
        {
            get
            {
                return new ViewModelCommand(AddExecute, AddCanExecute);
            }
        }

        private void AddExecute(object locality)
        {
            if (this.validate())
            {
                localityRepository.Add(Locality);
                MessageBox.Show("Localidad agregada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private bool validate()
        {
            if (string.IsNullOrEmpty(Locality.Code))
            {
                MessageBox.Show("Ingresa una clave", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(Locality.Comunidad))
            {
                MessageBox.Show("Ingresa una comunidad", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (Locality.Municipio == 0)
            {
                MessageBox.Show("Selecciona un municipio", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (Locality.Ambito == 0)
            {
                MessageBox.Show("Selecciona un ámbito", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private bool AddCanExecute(object locality)
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

        private void UpdateExecute(object locality)
        {
            if (this.validate())
            {
                localityRepository.Update(Locality);
                MessageBox.Show("Localidad actualizada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private bool UpdateCanExecute(object locality)
        {
            return true;
        }
    }
}
