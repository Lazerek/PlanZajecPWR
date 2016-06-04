using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PlanZajec.ViewModels;

namespace PlanZajec.Views
{
    /// <summary>
    /// Klasa pozwalająca na wybieranie opcji filtorwania grup
    /// </summary>
    public partial class FiltrujGrupy : UserControl
    {
        private PrzegladanieGrupViewModel viewModel;

        public FiltrujGrupy()
        {
            InitializeComponent();
            cb_wsz.IsChecked = true;
        }

        /// <summary>
        /// Metoda sprawdzająca czy wpisywany znak jest liczbą
        /// </summary>

        private void PreviewTextInput2(object sender, TextCompositionEventArgs e)
        {
            char c = Convert.ToChar(e.Text);
            if (Char.IsNumber(c))
                e.Handled = false;
            else
                e.Handled = true;
        }
        /// <summary>
        /// Obsługa przycisku filtruj
        /// </summary>

        private void filtruj_Click(object sender, RoutedEventArgs e)
        {
            Boolean lab = false, projekt = false, cwiczenia = false, wszystko = false, wyklad = false, wolne = false;
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
            if (cb_wolne.IsChecked == true)
                wolne = true;
            PrzegladanieGrupViewModel.przegladanieGrupViewModel.Filtruj2(nazwaK.Text, nazwaPot.Text, nazwaKG.Text, nazwaKK.Text, nazwaP.Text, wolneMiejsca.Text, lab, cwiczenia, projekt, wszystko, wyklad, wolne);
        }
        /// <summary>
        /// Metoda uruchamiająca filtrowanie po wyciśnięciu przycisku enter na polu
        /// </summary>

        private void filtrujEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                wlaczFiltr();
        }
        /// <summary>
        /// Metoda reagująca na zaznaczenie pola "wszystko" i ustawienie pozostałych checkboxów na true
        /// </summary>

        private void checkBoxWszystko_Checked(object sender, RoutedEventArgs e)
        {
            wlaczFiltr();
            cb_wyk.IsChecked = true;
            cb_lab.IsChecked = true;
            cb_pro.IsChecked = true;
            cb_cw.IsChecked = true;
        }
        /// <summary>
        /// Metoda uruchamiająca filtrowanie
        /// </summary>
        private void wlaczFiltr()
        {
            Boolean lab = false, projekt = false, cwiczenia = false, wszystko = false, wyklad = false, wolne = false;
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
            if (cb_wolne.IsChecked == true)
                wolne = true;
            PrzegladanieGrupViewModel.przegladanieGrupViewModel.Filtruj2(nazwaK.Text, nazwaPot.Text, nazwaKG.Text, nazwaKK.Text, nazwaP.Text, wolneMiejsca.Text, lab, cwiczenia, projekt, wszystko, wyklad, wolne);
        }
        /// <summary>
        /// Metoda obsługi zaznaczenia checkboxa
        /// </summary>

        private void check(object sender, RoutedEventArgs e)
        {
            wlaczFiltr();
        }
        /// <summary>
        /// Metoda obsługi odznaczenia checkboxa
        /// </summary>

        private void uncheck(object sender, RoutedEventArgs e)
        {
            cb_wsz.IsChecked = false;
            wlaczFiltr();
        }
        /// <summary>
        /// Metoda obsługi tylko wolnych zajęć
        /// </summary>

        private void pokazTylkoWolne(object sender, RoutedEventArgs e)
        {
            wlaczFiltr();
        }
        /// <summary>
        /// Metoda czyszcząca filtry
        /// </summary>

        private void wyczyscFiltr(object sender, RoutedEventArgs e)
        {
            cb_wsz.IsChecked = true;
            cb_wyk.IsChecked = true;
            cb_lab.IsChecked = true;        
            cb_pro.IsChecked = true;
            cb_cw.IsChecked = true;
            cb_wolne.IsChecked = false;
            nazwaK.Text = "";
            nazwaKG.Text = "";
            nazwaKK.Text = "";
            nazwaP.Text = "";
            nazwaPot.Text = "";
            wolneMiejsca.Text = "";
            PrzegladanieGrupViewModel.przegladanieGrupViewModel.czyscFiltrownie();
        }
    }
}
