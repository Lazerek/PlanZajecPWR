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
using PlanZajec.ViewModels;
namespace Wpf
{
    /// <summary>
    /// Interaction logic for PrzegladanieGrup.xaml
    /// </summary>
    /// 

    public partial class PrzegladanieGrup : UserControl
    {
        private PrzegladanieGrupViewModel viewModel;
        public PrzegladanieGrup()
        {
            InitializeComponent();
            viewModel = new PrzegladanieGrupViewModel();
            this.DataContext = viewModel;
        }

        private void dgUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
