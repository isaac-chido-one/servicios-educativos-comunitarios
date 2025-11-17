using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosEducativosComunitarios.Model
{
    public class LocalityModel : INotifyPropertyChanged
    {
        private int _id = 0;

        private string? _code;

        private int _municipio = 0;

        private string? _comunidad;

        private int _ambito = 0;

        private string? _latitud;

        private string? _longitud;

        private int _poblacion = 0;

        private bool _isDirty = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value, nameof(Id));
        }

        public string? Code
        {
            get => _code;
            set => SetProperty(ref _code, value, nameof(Code));
        }

        public int Municipio
        {
            get => _municipio;
            set => SetProperty(ref _municipio, value, nameof(Municipio));
        }

        public string? Comunidad
        {
            get => _comunidad;
            set => SetProperty(ref _comunidad, value, nameof(Comunidad));
        }

        public int Ambito
        {
            get => _ambito;
            set => SetProperty(ref _ambito, value, nameof(Ambito));
        }

        public string? Latitud
        {
            get => _latitud;
            set => SetProperty(ref _latitud, value, nameof(Latitud));
        }

        public string? Longitud
        {
            get => _longitud;
            set => SetProperty(ref _longitud, value, nameof(Longitud));
        }

        public int Poblacion
        {
            get => _poblacion;
            set => SetProperty(ref _poblacion, value, nameof(Poblacion));
        }

        // Indica si el modelo tiene cambios desde la última aceptación.
        public bool IsDirty
        {
            get => _isDirty;
            set
            {
                if (_isDirty == value)
                {
                    return;
                }

                _isDirty = value;
                OnPropertyChanged(nameof(IsDirty));
            }
        }

        // Marca que los cambios fueron aceptados (por ejemplo, después de guardar)
        public void AcceptChanges()
        {
            IsDirty = false;
        }

        protected void SetProperty<T>(ref T field, T value, string propertyName)
        {
            if (Equals(field, value))
            {
                return;
            }

            field = value;
            // Cuando una propiedad cambia marcamos IsDirty = true
            // (si la entidad se acaba de crear y Id==0 -> también será true mientras se edita)
            IsDirty = true;
            OnPropertyChanged(propertyName);
        }

        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
