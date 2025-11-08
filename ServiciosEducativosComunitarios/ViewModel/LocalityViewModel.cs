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
            localityRepository.Add(Locality);
            MessageBox.Show("Localidad agregada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private bool AddCanExecute(object user)
        {
            return true;
        }
    }
}
