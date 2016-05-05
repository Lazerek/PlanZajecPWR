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

namespace PlanZajec.Views
{
    /// <summary>
    /// Interaction logic for FiltrujGrupy.xaml
    /// </summary>
    public partial class FiltrujGrupy : UserControl
    {
        private PrzegladanieGrupViewModel viewModel;
        public FiltrujGrupy()
        {
            InitializeComponent();
            cb_wsz.IsChecked = true;
        }

        public void wyczyscFiltr(object sender, EventArgs e)
        {
            nazwaK.Text = "";
            nazwaP.Text = "";
            nazwaKG.Text = "";
            nazwaKK.Text = "";
            nazwaPot.Text = "";
            cb_wsz.IsChecked = true;
            cb_wyk.IsChecked = false;
            cb_lab.IsChecked = false;
            cb_pro.IsChecked = false;
            cb_cw.IsChecked = false;
        }

        private void filtruj_Click(object sender, RoutedEventArgs e)
        {
            Boolean lab = false, projekt = false, cwiczenia = false, wszystko = false, wyklad = false;
            if (cb_cw.IsChecked == true)
                cwiczenia = true;
            if (cb_lab.IsChecked == true)
                lab = true;
            if (cb_pro.IsChecked == true)
                projekt = true;
            if (cb_wyk.IsChecked == true)
                wyklad = true;
            if (cb_wsz.IsChecked == true)
                wszystko = true;
            PrzegladanieGrupViewModel.temp.Filtruj2(nazwaK.Text, nazwaPot.Text, nazwaKG.Text, nazwaKK.Text, nazwaP.Text, wolneMiejsca.Text, lab, cwiczenia, projekt, wszystko, wyklad);
        }

        private void filtrujEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                wlaczFiltr();
        }
        private void checkBoxWszystko_Checked(object sender, RoutedEventArgs e)
        {
            wlaczFiltr();
            cb_wyk.IsChecked = true;
            cb_lab.IsChecked = true;
            cb_pro.IsChecked = true;
            cb_cw.IsChecked = true;
        }
        private void wlaczFiltr()
        {
            Boolean lab = false, projekt = false, cwiczenia = false, wszystko = false, wyklad = false;
            if (cb_cw.IsChecked == true)
                cwiczenia = true;
            if (cb_lab.IsChecked == true)
                lab = true;
            if (cb_pro.IsChecked == true)
                projekt = true;
            if (cb_wyk.IsChecked == true)
                wyklad = true;
            if (cb_wsz.IsChecked == true)
                wszystko = true;
            PrzegladanieGrupViewModel.temp.Filtruj2(nazwaK.Text, nazwaPot.Text, nazwaKG.Text, nazwaKK.Text, nazwaP.Text, wolneMiejsca.Text, lab, cwiczenia, projekt, wszystko, wyklad);
        }
        private void check(object sender, RoutedEventArgs e)
        {
            wlaczFiltr();
        }
        private void uncheck(object sender, RoutedEventArgs e)
        {
            cb_wsz.IsChecked = false;
            wlaczFiltr();
        }

        private void wyczyscFiltr(object sender, RoutedEventArgs e)
        {
            cb_wsz.IsChecked = true;
            cb_wyk.IsChecked = true;
            cb_lab.IsChecked = true;
            cb_pro.IsChecked = true;
            cb_cw.IsChecked = true;
            nazwaK.Text = "";
            nazwaKG.Text = "";
            nazwaKK.Text = "";
            nazwaP.Text = "";
            nazwaPot.Text = "";
            wolneMiejsca.Text = "";
            PrzegladanieGrupViewModel.temp.czyscFiltrownie();
        }
    }
}
