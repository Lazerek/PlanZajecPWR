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
        private OknoOpcji _oknoOpcji;
        private readonly MainWindow _mainWindow;

        public PanelFiltrow(MainWindow mainWindow)
        {
            InitializeComponent();
            lpm = false;
            _oknoOpcji = new OknoOpcji();
            PrawePodmenu.Children.Add(new ProwadzacyMenu());
            _mainWindow = mainWindow;
        }

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
                if (!_oknoOpcji.IsVisible)
                {
                    _oknoOpcji = new OknoOpcji();
                    _oknoOpcji.Show();
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

        public void UpdateOnSelectedPlanChange(long? planNumber)
        {
            //PrawePodmenu.Children.Add(new ProwadzacyMenu());
            var prawePodmenu = PrawePodmenu.Children.OfType<PanelPrzegladaniaKafelekView>().FirstOrDefault();
            prawePodmenu?.UpdateOnSelectedPlanChange(planNumber);
        }
    }
}
