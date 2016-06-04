using PlanZajec.CommonInformations;
using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using PlanZajec.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace PlanZajec.Views
{
    /// <summary>
    /// Klasa typu window wyświetlająca kafelki z panelu
    /// </summary>
    public partial class PanelPrzegladaniaKafelekView : UserControl
    {
        private readonly Image addIcon;
        private readonly Image minusIcon;
        private GrupyZajeciowe grupaToAddOrDeleteFromPlan;
        private bool panelIsOpen;
        private PrzegladanieGrupViewModel viewModel;

        /// <summary>
        /// Domyślny konstruktor
        /// </summary>
        public PanelPrzegladaniaKafelekView()
        {
            InitializeComponent();
            addIcon = new Image() { Source = new BitmapImage(new Uri("Images/addIcon.png", UriKind.Relative)) };
            minusIcon = new Image() { Source = new BitmapImage(new Uri("Images/minusIcon.png", UriKind.Relative)) };
            panelIsOpen = false;
            cb_wsz.IsChecked = true;
            label2.Visibility = Visibility.Hidden;
            nazwaP.Visibility = Visibility.Hidden;
            Grid.SetRow(nazwaK, 0);
        }
        /// <summary>
        /// Metoda przygotowująca do otwarcia menu kontekstowego
        /// </summary>

        private void PrepareOpenContextMenu(object sender, ContextMenuEventArgs e)
        {
            KafelekGrup kafelek = sender as KafelekGrup;
            GrupyZajeciowe grupa = kafelek?.DataContext as GrupyZajeciowe;         
            FrameworkElement fe = e.Source as FrameworkElement;

            fe.ContextMenu = GetContextMenu(grupa);
        }

        /// <summary>
        /// Metoda zwracająca menu
        /// </summary>
        /// <param name="grupa">Grupa zajęciowa</param>
        /// <returns>Kontekstowe menu</returns>
        private ContextMenu GetContextMenu(GrupyZajeciowe grupa)
        {
            ContextMenu theMenu = new ContextMenu();
            if (grupa == null || ActChosenPlanSingleton.Instance.IdPlanu < 0)
            {
                theMenu.Visibility = Visibility.Hidden;
                return theMenu;
            }
            
            MenuItem menuAddOrRemovFromPlan = new MenuItem();

            using (var unitOfWork = new UnitOfWork(new PlanPwrContext()))
            {
                GrupyZajeciowe grupaUW = unitOfWork.GrupyZajeciowe.Get(grupa.KodGrupy);
                grupaToAddOrDeleteFromPlan = grupaUW;

                if(unitOfWork.GrupyZajeciowe.InThePlan(grupa.KodGrupy, ActChosenPlanSingleton.Instance.IdPlanu))
                {
                    menuAddOrRemovFromPlan.Header = "Usuń z planu";
                    menuAddOrRemovFromPlan.Icon = minusIcon;
                    menuAddOrRemovFromPlan.Click += new RoutedEventHandler(OnRemoveFromPlanHandler);             
                }
                else
                {
                    menuAddOrRemovFromPlan.Header = "Dodaj do planu";
                    menuAddOrRemovFromPlan.Icon = addIcon;
                    menuAddOrRemovFromPlan.Click += new RoutedEventHandler(OnAddToPlanHandler);
                    CommandManager.InvalidateRequerySuggested();
                }
            }
            theMenu.Items.Add(menuAddOrRemovFromPlan);
            return theMenu;
        }
        /// <summary>
        /// Obsługa na dodanie kafelka do planu
        /// </summary>

        private void OnAddToPlanHandler(object sender, RoutedEventArgs e)
        {
            using (var unitOfWork = new UnitOfWork(new PlanPwrContext()))
            {
                unitOfWork.Plany.DodajGrupeZajeciowaDoPlanu(grupaToAddOrDeleteFromPlan);
            }
            PlanView.RefreshSchedule();
        }
        /// <summary>
        /// Obsługa na usunięciu z planu
        /// </summary>

        private void OnRemoveFromPlanHandler(object sender, RoutedEventArgs e)
        {
            using (var unitOfWork = new UnitOfWork(new PlanPwrContext()))
            {
                unitOfWork.Plany.UsunGrupeZajeciowaZPlanu(grupaToAddOrDeleteFromPlan);
            }
            PlanView.RefreshSchedule();
        }

        private void panelClick(object sender, RoutedEventArgs e)
        {
            if (panelIsOpen)
            {
                panelButton.Content = "▼";
                zamknijPanel();
            }
            else
            {
                panelButton.Content = "▲";
                otworzPanel();
            }
        }
        /// <summary>
        /// Funkcja otwierająca panel
        /// </summary>
        private void otworzPanel()
        {
            panelIsOpen = true;
            filtry.Height = new GridLength(1, GridUnitType.Star);
            kafelki.Height = new GridLength(0, GridUnitType.Pixel);
            label2.Visibility = Visibility.Visible;
            nazwaP.Visibility = Visibility.Visible;
            Grid.SetRow(nazwaK, 1);
            nazwaK.Margin = new Thickness(0, 0, 0, 0);
        }
        /// <summary>
        /// Funkcja zamykająca panel
        /// </summary>
        private void zamknijPanel()
        {
            panelIsOpen = false;
            filtry.Height = new GridLength(45, GridUnitType.Pixel);
            kafelki.Height = new GridLength(1, GridUnitType.Star);
            label2.Visibility = Visibility.Hidden;
            nazwaP.Visibility = Visibility.Hidden;
            Grid.SetRow(nazwaK, 0);
            nazwaK.Margin = new Thickness(80, 0, 0, 0);
        }

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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void filtrujEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                wlaczFiltr();
        }
        /// <summary>
        /// Metoda reagująca na zaznaczenie pola "wszystko" i ustawienie pozostałych checkboxów na true
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void check(object sender, RoutedEventArgs e)
        {
            wlaczFiltr();
        }
        /// <summary>
        /// Metoda obsługi odznaczenia checkboxa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uncheck(object sender, RoutedEventArgs e)
        {
            cb_wsz.IsChecked = false;
            wlaczFiltr();
        }
        /// <summary>
        /// Metoda obsługi tylko wolnych zajęć
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pokazTylkoWolne(object sender, RoutedEventArgs e)
        {
            wlaczFiltr();
        }
        /// <summary>
        /// Metoda czyszcząca filtry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
