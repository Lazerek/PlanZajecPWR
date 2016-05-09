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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PlanZajec.Views
{
    /// <summary>
    /// Interaction logic for KafelPlanu.xaml
    /// </summary>
    public partial class KafelPlanu : UserControl
    {
        public KafelPlanu()
        {
            InitializeComponent();
        }

        private void Usun(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Polecenie usuniecia planu");
        }
    }
}
