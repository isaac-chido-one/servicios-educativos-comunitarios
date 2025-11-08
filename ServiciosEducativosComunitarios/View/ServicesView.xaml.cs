using ServiciosEducativosComunitarios.Model;
using ServiciosEducativosComunitarios.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ServiciosEducativosComunitarios.View
{
    /// <summary>
    /// Lógica de interacción para ServicesView.xaml
    /// </summary>
    public partial class ServicesView : Window
    {
        private readonly AppView appView;

        private ServiceModel? serviceModel;

        public ServicesView(AppView appView)
        {
            this.appView = appView;
            InitializeComponent();
        }

        private void ResetForm()
        {
            this.TxtId.Text = "0";
            this.TxtCode.Text = string.Empty;
            this.ComboLocality.SelectedIndex = 0;
            this.ComboProgram.SelectedIndex = 0;
            this.ComboPeriod.SelectedIndex = 0;
            this.ComboStatus.SelectedIndex = 0;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void BtnStore_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            appView.loadCatalogues(true);
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.Visibility == Visibility.Visible) 
            {
                _ = LoadLocalitiesAsync();
            }
        }

        private async Task LoadLocalitiesAsync()
        {
            ComboBoxItem item;
            this.ComboLocality.Items.Clear();
            item = new ComboBoxItem
            {
                Content = "Seleccionar...",
                Tag = "0"
            };
            this.ComboLocality.Items.Add(item);
            this.ComboLocality.SelectedIndex = 0;

            try
            {

                var repo = new LocalityRepository();
                // Ejecuta la consulta en un hilo de fondo
                var localities = await Task.Run(() => repo.GetAll());

                foreach (LocalityModel locality in localities)
                {
                    item = new ComboBoxItem
                    {
                        Content = locality.Code + ", " + locality.Comunidad,
                        Tag = locality.Id
                    };
                    this.ComboLocality.Items.Add(item);
                }
            }
            catch (System.Exception ex)
            {
                // Manejo simple: mostrar mensaje. Cambiar por logging si procede.
                MessageBox.Show($"Error cargando localidades: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (this.serviceModel != null)
            {
                this.TxtId.Text = this.serviceModel.Id.ToString();
                this.TxtCode.Text = this.serviceModel.Code;
                this.ComboLocality.SelectedValue = this.serviceModel.LocalityId;
                this.ComboProgram.SelectedValue = this.serviceModel.Program;
                this.ComboPeriod.SelectedValue = this.serviceModel.Period;
                this.ComboStatus.SelectedValue = this.serviceModel.Status;
            }
        }

        public void ShowNew()
        {
            this.ResetForm();
            this.serviceModel = null;
            this.BtnCreate.Visibility = Visibility.Visible;
            this.BtnUpdate.Visibility = Visibility.Collapsed;
            this.Show();
        }

        public void ShowEdit(ServiceModel serviceModel)
        {
            this.ResetForm();
            this.serviceModel = serviceModel;
            this.BtnCreate.Visibility = Visibility.Collapsed;
            this.BtnUpdate.Visibility = Visibility.Visible;
            this.Show();
        }

    }
}
