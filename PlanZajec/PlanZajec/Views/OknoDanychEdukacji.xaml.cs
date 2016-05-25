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

namespace PlanZajec.Views
{
    /// <summary>
    /// Interaction logic for OknoDanychEdukacji.xaml
    /// </summary>
    public partial class OknoDanychEdukacji : Window
    {
        public OknoDanychEdukacji()
        {
            InitializeComponent();
        }

        private void Sciagnij(object sender, RoutedEventArgs e)
        {

        }

        private void Anuluj(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
