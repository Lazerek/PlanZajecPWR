using PlanZajec.ViewModels;
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
using Wpf;

namespace PlanZajec.Views
{
    /// <summary>
    /// Interaction logic for OknoOpcji.xaml
    /// </summary>
    public partial class OknoOpcji : Window
    {
        public OknoOpcji()
        {
            InitializeComponent();
        }

        private void buttonSzukajProwadzacy_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Coś");
            PrzegladanieGrupViewModel.przegladanieGrupViewModel.PrzegladanieProwadzacy(textBoxprowadzacy.Text);
        }
    }
}
