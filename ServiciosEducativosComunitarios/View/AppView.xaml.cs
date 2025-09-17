using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
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
            this.localityView = new LocalityView();
            this.servicesView = new ServicesView();
        }

        private void MenuNewLocality_Click(object sender, RoutedEventArgs e)
        {
            this.localityView.Show();
        }

        private void MenuNewService_Click(object sender, RoutedEventArgs e)
        {
            this.servicesView.Show();
        }
    }
}
