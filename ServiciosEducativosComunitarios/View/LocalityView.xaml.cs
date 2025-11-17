using ServiciosEducativosComunitarios.Model;
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
    /// Lógica de interacción para LocalityView.xaml
    /// </summary>
    public partial class LocalityView : Window
    {
        private LocalityModel? localityModel;

        public LocalityView()
        {
            InitializeComponent();
        }

        private void ResetForm()
        {
            this.TxtId.Text = "0";
            this.TxtCode.Text = string.Empty;
            this.ComboMunicipio.SelectedIndex = 0;
            this.TxtName.Text = string.Empty;
            this.ComboScope.SelectedIndex = 0;
            this.TxtLatitude.Text = string.Empty;
            this.TxtLongitude.Text = string.Empty;
            this.TxtPopulation.Text = "0";
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void BtnStore_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        public void ShowNew()
        {
            this.ResetForm();
            this.localityModel = null;
            this.BtnCreate.Visibility = Visibility.Visible;
            this.BtnUpdate.Visibility = Visibility.Collapsed;
            this.Show();
        }

        public void ShowEdit(LocalityModel localityModel)
        {
            this.ResetForm();
            this.localityModel = localityModel;
            this.TxtId.Text = localityModel.Id.ToString();
            this.TxtCode.Text = localityModel.Code;
            this.ComboMunicipio.SelectedValue = localityModel.Municipio;
            this.TxtName.Text = localityModel.Comunidad;
            this.ComboScope.SelectedValue = localityModel.Ambito;
            this.TxtLatitude.Text = localityModel.Latitud;
            this.TxtLongitude.Text = localityModel.Longitud;
            this.TxtPopulation.Text = localityModel.Poblacion.ToString();
            this.BtnCreate.Visibility = Visibility.Collapsed;
            this.BtnUpdate.Visibility = Visibility.Visible;
            this.Show();
        }
    }
}
