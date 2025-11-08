using Microsoft.VisualBasic.ApplicationServices;
using ServiciosEducativosComunitarios.Model;
using ServiciosEducativosComunitarios.Repositories;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
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
        protected LocalityView localityView;

        protected ServicesView servicesView;
        public AppView()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            this.localityView = new LocalityView(this);
            this.servicesView = new ServicesView(this);
            loadCatalogues(false);
        }
        public void loadCatalogues(bool esperar)
        {
            // Cargar localities y servicios desde el repositorio sin bloquear la UI
            _ = LoadLocalitiesAsync(esperar);
            _ = LoadServicesAsync(esperar);
        }

        private async Task LoadLocalitiesAsync(bool esperar)
        {
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

        private void MenuNewLocality_Click(object sender, RoutedEventArgs e)
        {
            this.localityView.Show();
        }

        private void MenuNewService_Click(object sender, RoutedEventArgs e)
        {
            this.servicesView.Show();
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

        private void RadioButtonNotifications_Click(object sender, RoutedEventArgs e)
        {
            if (this.RadioButtonNotifications.IsChecked == true)
            {
                this.ContentControlMain.Visibility = Visibility.Visible;
                this.GridLocalities.Visibility = Visibility.Collapsed;
                this.GridServices.Visibility = Visibility.Collapsed;
            }
        }

        private void ButtonNewLocality_Click(object sender, RoutedEventArgs e)
        {
            this.localityView.ResetForm();
            this.localityView.Visibility = Visibility.Visible;
        }

        private void ButtonNewService_Click(object sender, RoutedEventArgs e)
        {
            this.servicesView.ResetForm();
            this.servicesView.Visibility = Visibility.Visible;
        }

        private void ButtonDeleteLocality_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            try
            {
                string tag = button.Tag.ToString();
                int id = int.Parse(tag);
                LocalityModel localityModel = new LocalityModel { Id = id };
                var repo = new LocalityRepository();
                Task.Run(() => repo.Delete(localityModel));
                _ = LoadLocalitiesAsync(true);
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
                string tag = button.Tag.ToString();
                int id = int.Parse(tag);
                ServiceModel serviceModel = new ServiceModel { Id = id };
                var repo = new ServiceRepository();
                Task.Run(() => repo.Delete(serviceModel));
                _ = LoadServicesAsync(true);
            }
            catch (Exception ex)
            {
            }
        }

    }
}
