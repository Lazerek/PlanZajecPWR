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
        bool lpm;
        OknoOpcji oo;
        private MainWindow parent;
        /// <summary>
        /// Domyślny konstruktor pokazujący okno opcji
        /// </summary>
        public PanelFiltrow()
        {
            InitializeComponent();
            lpm = false;
            oo = new OknoOpcji();
            PrawePodmenu.Children.Add(new ProwadzacyMenu());
        }
        /// <summary>
        /// Konstruktor z głownym oknem
        /// </summary>
        /// <param name="parent">Główne okno</param>
        public PanelFiltrow(MainWindow parent)
        {
            InitializeComponent();
            lpm = false;
            oo = new OknoOpcji();
            PrawePodmenu.Children.Add(new ProwadzacyMenu());
            this.parent = parent;
        }
        /// <summary>
        /// Metoda zmieniająca okno na okno z prowadzącymi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectedLecturersOpinion(object sender, RoutedEventArgs e)
        {
            if (PrawePodmenu == null)
                return;
            if (PrawePodmenu.Children.Count > 0)
                PrawePodmenu.Children.Remove(PrawePodmenu.Children[0]);
            PrawePodmenu.Children.Add(new ProwadzacyOpinie());
            
        }

        public void imgClick1(object sender, MouseEventArgs e)
        {
            lpm = true;
        }
        public void imgClick2(object sender, MouseEventArgs e)
        {
            if (lpm)
            {
                if (!oo.IsVisible)
                {
                    oo = new OknoOpcji();
                    oo.Show();
                }
            }
        }

        public void imgClickC(object sender, MouseEventArgs e)
        {
            lpm = false;
        }
        /// <summary>
        /// Metoda zmieniająca okno na okno z filtrowanie grup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gButtonClicked(object sender, RoutedEventArgs e)
        {
            if (parent.kolumna1.MinWidth == 15)
            {
                parent.kolumna1.MinWidth = 150;
                parent.kolumna1.MaxWidth = int.MaxValue;
                parent.kolumna1.Width = new GridLength(1, GridUnitType.Star);
                fButton.Content = "◀";
            }
            else
            {
                parent.kolumna1.MinWidth = 15;
                parent.kolumna1.MaxWidth = 15;
                parent.kolumna1.Width = new GridLength(15, GridUnitType.Pixel);
                fButton.Content = "▶";
            }
        }
        /// <summary>
        /// Metoda wyświetlająca okno z kontrolą kursów
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectedDodajWolny(object sender, RoutedEventArgs e)
        {
            if (PrawePodmenu == null)
                return;
            if (PrawePodmenu.Children.Count > 0)
                PrawePodmenu.Children.Remove(PrawePodmenu.Children[0]);
            PrawePodmenu.Children.Add(new WolneDniSetView());
        }
    }
}
