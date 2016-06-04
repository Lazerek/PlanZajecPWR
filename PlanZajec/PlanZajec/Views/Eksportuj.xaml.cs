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

namespace PlanZajec.Views
{
    /// <summary>
    /// Interaction logic for Eksportuj.xaml
    /// </summary>
    public partial class Eksportuj : Window
    {
        bool typ;
        public Eksportuj()
        {
            InitializeComponent();
        }
        public Eksportuj(bool typ)
        {
            InitializeComponent();
            this.DataContext = PlanyViewModel.Instance;
            this.typ = typ;
            if (typ)
            {
                tekst.Visibility = Visibility.Hidden;
                format.Visibility = Visibility.Hidden;
                Grid.SetRow(anulujButton, 4);
                Grid.SetRow(eksportujButton, 4);
                mGrid.RowDefinitions[2].Height = new GridLength(40);
                eksportujButton.Content = "Eksportuj PDF";
                eksportujButton.Width = 100;
                this.Height = 230;
                this.Title = "Eksportuj jako plik PDF";
            }
        }

        private void Eksport(object sender, RoutedEventArgs e)
        {
            if (typ)
            {
                //TODO Przygotowanie pliku PDF -> zapis do pliku
            }
            else
            {
                //TODO Eksportuj jako plik graficzny
                /*
                if(format.SelectedIndex == 0){
                    Eksport JPG
                }else{
                    if(format.SelectedIndex == 1)...
                }
                Formaty w xaml tam gdzie jest typ
                */
            }
        }

        private void wyborSciezki(object sender, RoutedEventArgs e)
        {

        }

        private void Anuluj(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}