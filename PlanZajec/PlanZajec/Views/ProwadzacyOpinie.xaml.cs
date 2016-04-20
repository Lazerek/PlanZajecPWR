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
using PlanZajec.DataModel;

namespace PlanZajec
{
    /// <summary>
    /// Interaction logic for ProwadzacyMenu.xaml
    /// </summary>
    public partial class ProwadzacyOpinie : UserControl
    {
        public ProwadzacyOpinieViewModel ViewModel;
        public ProwadzacyOpinie()
        {
            InitializeComponent();
            ViewModel = new ProwadzacyOpinieViewModel();
            DataContext = ViewModel;
            foreach(Prowadzacy pr in ViewModel.ComboBoxItems)
                comboBox.Items.Add(pr.Tytul + " " + pr.Imie + " " + pr.Nazwisko);
            
            int rowIndex = comboBox.SelectedIndex;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void onChange(object sender, RoutedEventArgs e)
        {
           string[] wynik= ViewModel.dajOpinie(comboBox.SelectedValue+"");
           textBox.Text = wynik[0];
           Ocena.Text = wynik[1];

        }
    }
}
