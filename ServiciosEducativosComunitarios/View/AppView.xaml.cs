using Microsoft.VisualBasic.ApplicationServices;
using ServiciosEducativosComunitarios.Model;
using ServiciosEducativosComunitarios.Repositories;
using ServiciosEducativosComunitarios.ViewModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ServiciosEducativosComunitarios.View
{
    /// <summary>
    /// Interaction logic for AppView.xaml
    /// </summary>
    public partial class AppView : Window
    {
        // Colección observable de localidades usada por los ComboBoxes de los DataGrid
        public ObservableCollection<LocalityModel> Localities { get; } = new ObservableCollection<LocalityModel>();

        public AppView()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

            // Asegurar ItemsSource del grid de localidades a la colección observable
            this.DataGridLocalities.ItemsSource = this.Localities;

            // Cargar localities y servicios desde el repositorio sin bloquear la UI
            _ = LoadLocalitiesAsync();
        }

        private async Task LoadLocalitiesAsync()
        {
            this.LabelLocalityWarning.Text = "";
            this.LabelLocalityWarning.Visibility = Visibility.Collapsed;

            try
            {
                LocalityRepository repo = new LocalityRepository();
                // Ejecuta la consulta en un hilo de fondo
                var localities = await Task.Run(() => repo.GetAll().ToList());
                // Asigna el ItemsSource en el hilo de la UI
                this.DataGridLocalities.ItemsSource = localities;

                // Actualiza la colección observable en el hilo de la UI
                this.Localities.Clear();

                // placeholder con Id = 0 para mostrar "Seleccionar..." en los ComboBox
                this.Localities.Add(new LocalityModel
                {
                    Id = 0,
                    Code = "Seleccionar...",
                    Comunidad = string.Empty,
                    Municipio = 0,
                    Ambito = 0,
                    Latitud = string.Empty,
                    Longitud = string.Empty,
                    Poblacion = 0,
                    IsDirty = false
                });

                foreach (var locality in localities)
                {
                    this.Localities.Add(locality);
                }
            }
            catch (System.Exception ex)
            {
                ShowLocalityWarning($"Error cargando localidades: {ex.Message}");
            }

            _ = LoadServicesAsync();
        }

        private async Task LoadServicesAsync()
        {
            this.LabelServiceWarning.Text = "";
            this.LabelServiceWarning.Visibility = Visibility.Collapsed;

            try
            {
                ServiceRepository repo = new ServiceRepository();
                // Ejecuta la consulta en un hilo de fondo
                var services = await Task.Run(() => repo.GetAll().ToList());
                // Asigna el ItemsSource en el hilo de la UI
                this.DataGridServices.ItemsSource = services;
            }
            catch (System.Exception ex)
            {
                ShowServiceWarning($"Error cargando servicios: {ex.Message}");
            }
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void pnlControlBar_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161, 2, 0);
        }

        private void pnlControlBar_MouseEnter(object sender, MouseEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else this.WindowState = WindowState.Normal;
        }

        private void RadioButtonLocalities_Checked(object sender, RoutedEventArgs e)
        {
            if (this.RadioButtonLocalities.IsChecked == true)
            {
                this.ContentControlMain.Visibility = Visibility.Collapsed;
                this.GridLocalities.Visibility = Visibility.Visible;
                this.GridServices.Visibility = Visibility.Collapsed;
            }
        }

        private void RadioButtonServices_Checked(object sender, RoutedEventArgs e)
        {
            if (this.RadioButtonServices.IsChecked == true)
            {
                this.ContentControlMain.Visibility = Visibility.Collapsed;
                this.GridLocalities.Visibility = Visibility.Collapsed;
                this.GridServices.Visibility = Visibility.Visible;
            }
        }

        private void RadioButtonHome_Checked(object sender, RoutedEventArgs e)
        {
            if (this.RadioButtonHome.IsChecked == true)
            {
                this.ContentControlMain.Visibility = Visibility.Visible;
                this.GridLocalities.Visibility = Visibility.Collapsed;
                this.GridServices.Visibility = Visibility.Collapsed;
            }
        }

        private void ButtonNewService_Click(object sender, RoutedEventArgs e)
        {
            List<ServiceModel> services = (List<ServiceModel>)this.DataGridServices.ItemsSource;
            services.Insert(0, new ServiceModel
            {
                Id = 0,
                Code = string.Empty,
                LocalityId = 0,
                Locality = string.Empty,
                Period = 0,
                Program = 0,
                Status = 0,
                IsDirty = false
            });
            this.DataGridServices.ItemsSource = null;
            this.DataGridServices.ItemsSource = services;
        }

        private async void DeleteLocality(LocalityModel localityModel)
        {
            MessageBoxResult result = MessageBox.Show("¿Deseas eliminar la localidad?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            if (localityModel.Id == 0)
            {
                List<LocalityModel> localities = (List<LocalityModel>)this.DataGridLocalities.ItemsSource;
                localities.Remove(localityModel);
                this.DataGridLocalities.ItemsSource = null;
                this.DataGridLocalities.ItemsSource = localities;

                // eliminar elemento temporal/pendiente de la colección observable
                if (this.Localities.Contains(localityModel))
                {
                    this.Localities.Remove(localityModel);
                }

                return;
            }

            try
            {
                LocalityRepository localityRepository = new LocalityRepository();
                bool hasServices = await Task.Run(() => localityRepository.HasServices(localityModel));

                if (hasServices)
                {
                    ShowLocalityWarning("No se puede eliminar la localidad debido que está asociada con algunos servicios");

                    return;
                }

                await Task.Run(() => localityRepository.Delete(localityModel));
            }
            catch (Exception ex)
            {
                ShowLocalityWarning($"Error al eliminar localidad: {ex.Message}");

                return;
            }

            _ = LoadLocalitiesAsync();
            MessageBox.Show("Localidad eliminada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async void DeleteService(ServiceModel serviceModel)
        {
            MessageBoxResult result = MessageBox.Show("¿Deseas eliminar el servicio?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            if (serviceModel.Id == 0)
            {
                List<ServiceModel> services = (List<ServiceModel>)this.DataGridServices.ItemsSource;
                services.Remove(serviceModel);
                this.DataGridServices.ItemsSource = null;
                this.DataGridServices.ItemsSource = services;
                return;
            }

            try
            {
                ServiceRepository serviceRepository = new ServiceRepository();
                await Task.Run(() => serviceRepository.Delete(serviceModel));
            }
            catch (Exception ex)
            {
                ShowServiceWarning($"Error al eliminar servicio: {ex.Message}");

                return;
            }

            _ = LoadServicesAsync();
            MessageBox.Show("Servicio eliminado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void ButtonDeleteLocality_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button || button.DataContext is not LocalityModel localityModel)
            {
                return;
            }

            DeleteLocality(localityModel);
        }

        private void ButtonDeleteService_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button || button.DataContext is not ServiceModel serviceModel)
            {
                return;
            }

            DeleteService(serviceModel);
        }

        private void ShowLocalityWarning(string message)
        {
            this.LabelLocalityWarning.Text = "▲ " + message;
            this.LabelLocalityWarning.Visibility = Visibility.Visible;
            SystemSounds.Beep.Play();
        }

        private void ShowServiceWarning(string message)
        {
            this.LabelServiceWarning.Text = "▲ " + message;
            this.LabelServiceWarning.Visibility = Visibility.Visible;
            SystemSounds.Beep.Play();
        }

        // Guardar (Agregar/Actualizar) localidad desde el botón "Guardar" del DataGrid.
        private async void ButtonLocalityStore_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
            {
                return;
            }

            if (button.DataContext is not LocalityModel locality)
            {
                ShowLocalityWarning("Ingesa la información de la localidad");
                return;
            }

            if (String.IsNullOrEmpty(locality.Code))
            {
                ShowLocalityWarning("Ingesa un código de localidad");
                return;
            }

            string alphanumericPattern = @"^[a-zA-Z0-9]+$";

            if (!Regex.IsMatch(locality.Code, alphanumericPattern))
            {
                ShowLocalityWarning("El formato del código no es alfanumérico");
                return;
            }

            if (locality.Municipio == 0)
            {
                ShowLocalityWarning("Selecciona un municipio");
                return;
            }

            if (String.IsNullOrEmpty(locality.Comunidad))
            {
                ShowLocalityWarning("Ingesa un nombre de comunidad");
                return;
            }

            if (locality.Ambito == 0)
            {
                ShowLocalityWarning("Selecciona un ámbito");
                return;
            }

            string numericPattern = @"^[+-]?\d*([.,]\d+)?$";

            if (!String.IsNullOrEmpty(locality.Latitud) && !Regex.IsMatch(locality.Latitud, numericPattern))
            {
                ShowLocalityWarning("El formato de la latitud no es numérico");
                return;
            }

            if (!String.IsNullOrEmpty(locality.Longitud) && !Regex.IsMatch(locality.Longitud, numericPattern))
            {
                ShowLocalityWarning("El formato de la longitud no es numérico");
                return;
            }

            if (locality.Poblacion < 0)
            {
                ShowLocalityWarning("El campo población debe ser un número positivo");
                return;
            }

            try
            {
                LocalityRepository repo = new LocalityRepository();
                bool codeExists = await Task.Run(() => repo.CodeExists(locality));

                if (codeExists)
                {
                    ShowLocalityWarning("El código de localidad ya existe. Ingresa otro.");
                    return;
                }

                if (locality.Id == 0)
                {
                    // Nuevo registro
                    await Task.Run(() => repo.Add(locality));
                }
                else
                {
                    // Registro existente
                    await Task.Run(() => repo.Update(locality));
                }

                // Después de guardar, marcar como no modificado y refrescar catálogo
                locality.AcceptChanges();
                _ = LoadLocalitiesAsync();
                MessageBox.Show("Localidad guardada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                ShowLocalityWarning($"Error al guardar la localidad: {ex.Message}");
            }
        }

        // Manejador para eliminar filas con la tecla Delete en el DataGrid
        private void DataGridLocalities_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Delete)
            {
                return;
            }

            var selected = this.DataGridLocalities.SelectedItems.Cast<object>().ToList();
            if (selected == null || selected.Count == 0)
            {
                return;
            }

            LocalityModel? localityModel = selected.OfType<LocalityModel>().First();

            if (localityModel != null)
            {
                DeleteLocality(localityModel);
            }

            // Evitar que el DataGrid intente aplicar su propia lógica de borrado
            e.Handled = true;
        }

        private void DataGridServices_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Delete)
            {
                return;
            }

            var selected = this.DataGridServices.SelectedItems.Cast<object>().ToList();
            if (selected == null || selected.Count == 0)
            {
                return;
            }

            ServiceModel? serviceModel = selected.OfType<ServiceModel>().First();

            if (serviceModel != null)
            {
                DeleteService(serviceModel);
            }

            // Evitar que el DataGrid intente aplicar su propia lógica de borrado
            e.Handled = true;
        }

        private async void ButtonServiceStore_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
            {
                return;
            }

            if (button.DataContext is not ServiceModel serviceModel)
            {
                ShowServiceWarning("Ingesa la información del servicio");
                return;
            }

            if (String.IsNullOrEmpty(serviceModel.Code))
            {
                ShowServiceWarning("Ingesa una clave de servicio");
                return;
            }

            string alphanumericPattern = @"^[a-zA-Z0-9]+$";

            if (!Regex.IsMatch(serviceModel.Code, alphanumericPattern))
            {
                ShowServiceWarning("El formato del código no es alfanumérico");
                return;
            }

            if (serviceModel.LocalityId == 0)
            {
                ShowServiceWarning("Selecciona una localidad");
                return;
            }

            if (serviceModel.Period == 0)
            {
                ShowServiceWarning("Selecciona un periodo");
                return;
            }

            if (serviceModel.Program == 0)
            {
                ShowServiceWarning("Selecciona un programa");
                return;
            }

            if (serviceModel.Status == 0)
            {
                ShowServiceWarning("Selecciona un estatus");
                return;
            }

            try
            {
                ServiceRepository repo = new ServiceRepository();
                bool codeExists = await Task.Run(() => repo.CodeExists(serviceModel));

                if (codeExists)
                {
                    ShowServiceWarning("El código del servicio ya existe. Ingresa otro.");
                    return;
                }

                bool serviceExists = await Task.Run(() => repo.ServiceExists(serviceModel));

                if (serviceExists)
                {
                    ShowServiceWarning("El servicio ya existe. Ingresa información diferente.");
                    return;
                }

                if (serviceModel.Id == 0)
                {
                    // Nuevo registro
                    await Task.Run(() => repo.Add(serviceModel));
                }
                else
                {
                    // Registro existente
                    await Task.Run(() => repo.Update(serviceModel));
                }
            }
            catch (Exception ex)
            {
                ShowServiceWarning($"Error al guardar el servicio: {ex.Message}");
                return;
            }

            // Después de guardar, marcar como no modificado y refrescar catálogo
            serviceModel.AcceptChanges();
            _ = LoadServicesAsync();
            MessageBox.Show("Servicio guardado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ButtonNewLocality_Click(object sender, RoutedEventArgs e)
        {
            List<LocalityModel> localities = (List<LocalityModel>)this.DataGridLocalities.ItemsSource;
            localities.Insert(0, new LocalityModel
            {
                Id = 0,
                Code = string.Empty,
                Municipio = 0,
                Comunidad = string.Empty,
                Ambito = 0,
                Latitud = string.Empty,
                Longitud = string.Empty,
                Poblacion = 0,
                IsDirty = false
            });
            this.DataGridLocalities.ItemsSource = null;
            this.DataGridLocalities.ItemsSource = localities;
        }
    }
}
