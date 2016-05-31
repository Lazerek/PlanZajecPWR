using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using PlanZajec.ViewModels;

namespace PlanZajec
{
    /// <summary>
    ///     Klasa wyświetlająca opinie oraz oceny o prowadzących
    /// </summary>
    public partial class ProwadzacyOpinie : UserControl
    {
        private readonly int[] tab;
        public ProwadzacyOpinieViewModel ViewModel;
        /// <summary>
        /// Domyślny konstuktor, wypełnia dane combobox'a prowadzącymi
        /// </summary>
        public ProwadzacyOpinie()
        {
            InitializeComponent();
            ViewModel = new ProwadzacyOpinieViewModel();
            DataContext = ViewModel;
            var i = 0;
            tab = new int[ViewModel.ComboBoxItems.Count()];
            foreach (var pr in ViewModel.ComboBoxItems)
            {
                comboBox.Items.Add(pr.Tytul + " " + pr.Imie + " " + pr.Nazwisko);
                tab[i] = (int) pr.IdProwadzacego;
                i++;
            }
            var rowIndex = comboBox.SelectedIndex;
            comboBox.SelectedIndex = 0;
        }
        /// <summary>
        /// Metoda pozwalająca zapisać opinie oraz ocenę o prowadzącym
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Zapisz(object sender, RoutedEventArgs e)
        {
            var changeText = false;
            var rowIndex = comboBox.SelectedIndex;
            var ocena = Ocena.Text;
            ocena = ocena.Replace('.', ',');
            changeText = ViewModel.ZapiszOpinie(comboBox.SelectedIndex + "", tab[rowIndex], textBox.Text, ocena);
            if (changeText)
            {
                OcenaLabel.Content = "Niedozwolona ocena!";
                OcenaLabel.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                OcenaLabel.Content = "Zapisano!";
                OcenaLabel.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }
        }
        /// <summary>
        /// Metoda aktualizująca tekst po zmianie prowadzącego.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onChange(object sender, RoutedEventArgs e)
        {
            OcenaLabel.Content = "Wpisz ocenę od 2,0 do 5,5";
            OcenaLabel.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            var wynik = ViewModel.dajOpinie(comboBox.SelectedValue + "");
            textBox.Text = wynik[0];
            Ocena.Text = wynik[1];
        }
        /// <summary>
        /// Metoda sprawdzająca poprawność wpisanych znaków w ocenie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviewTextInput2(object sender, TextCompositionEventArgs e)
        {
            var stringToCheck = Ocena.Text;
            var c = Convert.ToChar(e.Text);
            if ((char.IsNumber(c) || c.Equals(',') || c.Equals('.')) && !stringToCheck.Contains(',') && !stringToCheck.Contains("."))
                e.Handled = false;
            else if (char.IsNumber(c))
                e.Handled = false;
            else
                e.Handled = true;
        }
    }
}