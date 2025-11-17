using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosEducativosComunitarios.Model
{
    public class ServiceModel : INotifyPropertyChanged
    {
        private int _id = 0;

        private string? _code;

        private int _localityid = 0;

        private int _period = 0;

        private int _program = 0;

        private int _status = 0;

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
        public int LocalityId 
        {
            get => _localityid;
            set => SetProperty(ref _localityid, value, nameof(LocalityId)); 
        }
        public string? Locality { get; set; }
        public int Period 
        {
            get => _period;
            set => SetProperty(ref _period, value, nameof(Period)); 
        }
        public int Program 
        {
            get => _program;
            set => SetProperty(ref _program, value, nameof(Program)); 
        }
        public int Status 
        {
            get => _status;
            set => SetProperty(ref _status, value, nameof(Status)); 
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
