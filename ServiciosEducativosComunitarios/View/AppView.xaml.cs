using Microsoft.VisualBasic.ApplicationServices;
using ServiciosEducativosComunitarios.Model;
using ServiciosEducativosComunitarios.Repositories;
using ServiciosEducativosComunitarios.ViewModel;
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
        protected ServicesView servicesView;
        public AppView()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            this.servicesView = new ServicesView(this);
            loadCatalogues(false);
        }
        public void loadCatalogues(bool esperar)
        {
            // Cargar localities y servicios desde el repositorio sin bloquear la UI
            _ = LoadLocalitiesAsync(esperar);
            _ = LoadServicesAsync(esperar);
        }

        private async Task LoadLocalitiesAsync(bool esperar = false)
        {
            this.LabelLocalityWarning.Text = "";
            this.LabelLocalityWarning.Visibility = Visibility.Collapsed;

            if (esperar)
            {
                await Task.Delay(1000);
            }

            try
            {
                var repo = new LocalityRepository();
                // Ejecuta la consulta en un hilo de fondo
                var localities = await Task.Run(() => repo.GetAll().ToList());
                // Asigna el ItemsSource en el hilo de la UI
                this.DataGridLocalities.ItemsSource = localities;
            }
            catch (System.Exception ex)
            {
                // Manejo simple: mostrar mensaje. Cambiar por logging si procede.
                MessageBox.Show($"Error cargando localidades: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadServicesAsync(bool esperar)
        {
            if (esperar)
            {
                await Task.Delay(1000);
            }

            try
            {
                var repo = new ServiceRepository();
                // Ejecuta la consulta en un hilo de fondo
                var services = await Task.Run(() => repo.GetAll().ToList());
                // Asigna el ItemsSource en el hilo de la UI
                this.DataGridServices.ItemsSource = services;
            }
            catch (System.Exception ex)
            {
                // Manejo simple: mostrar mensaje. Cambiar por logging si procede.
                MessageBox.Show($"Error cargando servicios: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            this.servicesView.ShowNew();
        }

        private async void ButtonDeleteLocality_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            try
            {
                string tag = button.Tag.ToString();
                int id = int.Parse(tag);
                LocalityModel localityModel = new LocalityModel { Id = id };
                var repo = new LocalityRepository();
                await Task.Run(() =>
                {
                    repo.Delete(localityModel);
                });
                _ = LoadLocalitiesAsync();
                MessageBox.Show("Localidad eliminada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
            }
        }

        private void ButtonDeleteService_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            try
            {
                string? tag = button.Tag.ToString();
                int id = tag == null ? 0 : int.Parse(tag);
                ServiceModel serviceModel = new ServiceModel { Id = id };
                var repo = new ServiceRepository();
                Task.Run(() => repo.Delete(serviceModel));
                MessageBox.Show("Servicio eliminado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                _ = LoadServicesAsync(true);
            }
            catch (Exception ex)
            {
            }
        }

        private void ButtonEditService_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (button.DataContext is ServiceModel serviceModel)
            {
                this.servicesView.ShowEdit(serviceModel);
            }
        }

        private void ShowLocalityWarning(string message)
        {
            this.LabelLocalityWarning.Text = message;
            this.LabelLocalityWarning.Visibility = Visibility.Visible;
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
                var repo = new LocalityRepository();
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
        private async void DataGridLocalities_PreviewKeyDown(object sender, KeyEventArgs e)
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

            if (localityModel == null || localityModel.Id == 0)
            {
                e.Handled = true;
                return;
            }

            LocalityRepository localityRepository = new LocalityRepository();
            List<string> errors = new List<string>();

            // Ejecutar en hilo de fondo las eliminaciones en BD
            await Task.Run(() =>
            {
                try
                {
                    localityRepository.Delete(localityModel);
                }
                catch (System.Exception ex)
                {
                    lock (errors)
                    {
                        errors.Add($"Id {localityModel.Id}: {ex.Message}");
                    }
                }
            });

            // Evitar que el DataGrid intente aplicar su propia lógica de borrado
            e.Handled = true;

            // Recargar catálogo para reflejar cambios
            _ = LoadLocalitiesAsync();

            if (errors.Any())
            {
                ShowLocalityWarning($"No se pudo eliminar la localidad: {string.Join("\n", errors)}");
            }
            else
            {
                MessageBox.Show("Localidad eliminada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
