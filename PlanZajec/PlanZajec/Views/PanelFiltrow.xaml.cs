using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PlanZajec;
using PlanZajec.Views;
using PlanZajec.ViewModels;

namespace Wpf
{
    /// <summary>
    /// Klasa wyświetlająca wybrany window w lewym panelu
    /// </summary>
    public partial class PanelFiltrow : UserControl
    {
        private bool lpm;
        private readonly MainWindow _mainWindow;
        /// <summary>
        /// Domyślny konstruktor
        /// </summary>
        public PanelFiltrow(MainWindow mainWindow)
        {
            InitializeComponent();
            lpm = false;
            PrawePodmenu.Children.Add(new ProwadzacyMenu());
            _mainWindow = mainWindow;
        }

        /// <summary>
        /// Metoda zmieniająca okno na okno z listą prowadzących
        /// </summary>
        /// 
        private void OnSelectedLecturers(object sender, RoutedEventArgs e)
        {
            if (PrawePodmenu == null) return;
            if (PrawePodmenu.Children.Count > 0)
            {
                PrawePodmenu.Children.Remove(PrawePodmenu.Children[0]);
            }
            PrawePodmenu.Children.Add(new ProwadzacyMenu());
        }
        /// <summary>
        /// Metoda zmieniajaca okno na okno z usuwaniem planów
        /// </summary>

        private void OnSelectedUsun(object sender, RoutedEventArgs e)
        {
            if (PrawePodmenu == null) return;
            if (PrawePodmenu.Children.Count > 0)
            {
                PrawePodmenu.Children.Remove(PrawePodmenu.Children[0]);
            }
            PrawePodmenu.Children.Add(new UsunPlan());
        }

        /// <summary>
        /// Metoda zmieniająca okno na okno ze zmianą opinii prowadzących
        /// </summary>

        private void OnSelectedLecturersOpinion(object sender, RoutedEventArgs e)
        {
            if (PrawePodmenu == null)
                return;
            if (PrawePodmenu.Children.Count > 0)
                PrawePodmenu.Children.Remove(PrawePodmenu.Children[0]);
            PrawePodmenu.Children.Add(new ProwadzacyOpinie());
            
        }
        /// <summary>
        /// Metoda zmieniająca okno na okno z filtrowanie grup
        /// </summary>

        public void OnSelectedFiltrujGrupy(object sender, RoutedEventArgs e)
        {
            if (PrawePodmenu == null) return;
            if (PrawePodmenu.Children.Count > 0)
            {
                PrawePodmenu.Children.Remove(PrawePodmenu.Children[0]);
            }
            PrawePodmenu.Children.Add(new FiltrujGrupy());

        }
        /// <summary>
        /// Metoda zmieniająca okno na okno z wybranymi grupami
        /// </summary>

        private void OnSelectedGrupy(object sender, RoutedEventArgs e)
        {
            if (PrawePodmenu == null) return;
            if (PrawePodmenu.Children.Count > 0)
            {
                PrawePodmenu.Children.Remove(PrawePodmenu.Children[0]);
            }
            PrawePodmenu.Children.Add(preparePrzegladanieKafelkow());
        }
        /// <summary>
        /// Metoda przygotowująca do przeglądania kafelków
        /// </summary>
        /// <returns></returns>
        private UIElement preparePrzegladanieKafelkow()
        {
            return new PanelPrzegladaniaKafelekView()
                { DataContext = PrzegladanieGrupViewModel.przegladanieGrupViewModel};
        }
        /// <summary>
        /// Metoda chowająca i wyświetlająca okno
        /// </summary>

        private void gButtonClicked(object sender, RoutedEventArgs e)
        {
            if (_mainWindow.kolumna1.MinWidth == 15)
            {
                _mainWindow.kolumna1.MinWidth = 150;
                _mainWindow.kolumna1.MaxWidth = int.MaxValue;
                _mainWindow.kolumna1.Width = new GridLength(1, GridUnitType.Star);
                fButton.Content = "◀";
            }
            else
            {
                _mainWindow.kolumna1.MinWidth = 15;
                _mainWindow.kolumna1.MaxWidth = 15;
                _mainWindow.kolumna1.Width = new GridLength(15, GridUnitType.Pixel);
                fButton.Content = "▶";
            }
        }
        /// <summary>
        /// Metoda wyświetlająca okno z kontrolą kursów
        /// </summary>

        private void KontrolaKursowComboItem_OnSelected(object sender, RoutedEventArgs e)
        {
            if (PrawePodmenu == null)
                return;
            if (PrawePodmenu.Children.Count > 0)
                PrawePodmenu.Children.Remove(PrawePodmenu.Children[0]);
            PrawePodmenu.Children.Add(new KontrolaZapisowView());
        }
        /// <summary>
        /// Metoda wyświetlajaca okno z dodaniem wolnych dni
        /// </summary>

        private void OnSelectedDodajWolny(object sender, RoutedEventArgs e)
        {
            if (PrawePodmenu == null)
                return;
            if (PrawePodmenu.Children.Count > 0)
                PrawePodmenu.Children.Remove(PrawePodmenu.Children[0]);
            PrawePodmenu.Children.Add(new WolneDniView());
        }
    }
}
