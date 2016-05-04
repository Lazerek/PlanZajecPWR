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
        private int[] tab;
        public ProwadzacyOpinie()
        {
            InitializeComponent();
            ViewModel = new ProwadzacyOpinieViewModel();
            DataContext = ViewModel;
            int i = 0;
            tab = new int[ViewModel.ComboBoxItems.Count()];
            foreach (Prowadzacy pr in ViewModel.ComboBoxItems)
            {
                comboBox.Items.Add(pr.Tytul + " " + pr.Imie + " " + pr.Nazwisko);
                tab[i] = (int)pr.IdProwadzacego;
                i++;
            }
            int rowIndex = comboBox.SelectedIndex;
        }

        private void Zapisz(object sender, RoutedEventArgs e)
        {
            Boolean changeText = false;
            int rowIndex = comboBox.SelectedIndex;
            String ocena = Ocena.Text;
            ocena = ocena.Replace('.', ',');
            changeText = ViewModel.ZapiszOpinie(comboBox.SelectedIndex + "", tab[rowIndex], textBox.Text, ocena);
            if (changeText)
            {
                OcenaLabel.Content = "Niedozwolona ocena!";
                OcenaLabel.Foreground = new SolidColorBrush(Colors.Red);
            }else
            {
                OcenaLabel.Content = "Zapisano!";
                OcenaLabel.Foreground = new SolidColorBrush(Color.FromRgb(0,0,0));
            }
        }
        private void onChange(object sender, RoutedEventArgs e)
        {
            OcenaLabel.Content = "Wpisz ocenę od 2,0 do 5,5";
            OcenaLabel.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            string[] wynik= ViewModel.dajOpinie(comboBox.SelectedValue+"");
           textBox.Text = wynik[0];
           Ocena.Text = wynik[1];
        }
    }
}
