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
        private readonly AppView appView;
        public LocalityView(AppView appView)
        {
            this.appView = appView;
            InitializeComponent();
        }

        public void ResetForm()
        {
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
            appView.loadCatalogues(true);
        }
    }
}
