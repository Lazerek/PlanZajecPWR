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
using PlanZajec;
using PlanZajec.Views;
using PlanZajec.ViewModels;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for PraweMenu.xaml
    /// </summary>
    public partial class PanelFiltrow : UserControl
    {
        bool lpm;
        OknoOpcji oo;
        private MainWindow parent;

        public PanelFiltrow()
        {
            InitializeComponent();
            lpm = false;
            oo = new OknoOpcji();
            PrawePodmenu.Children.Add(new ProwadzacyMenu());
        }

        public PanelFiltrow(MainWindow parent)
        {
            InitializeComponent();
            lpm = false;
            oo = new OknoOpcji();
            PrawePodmenu.Children.Add(new ProwadzacyMenu());
            this.parent = parent;
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

        private void OnSelectedUsun(object sender, RoutedEventArgs e)
        {
            if (PrawePodmenu == null) return;
            if (PrawePodmenu.Children.Count > 0)
            {
                PrawePodmenu.Children.Remove(PrawePodmenu.Children[0]);
            }
            PrawePodmenu.Children.Add(new UsunPlan());
        }


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

        public void OnSelectedFiltrujGrupy(object sender, RoutedEventArgs e)
        {
            if (PrawePodmenu == null) return;
            if (PrawePodmenu.Children.Count > 0)
            {
                PrawePodmenu.Children.Remove(PrawePodmenu.Children[0]);
            }
            PrawePodmenu.Children.Add(new FiltrujGrupy());

        }

        private void OnSelectedGrupy(object sender, RoutedEventArgs e)
        {
            if (PrawePodmenu == null) return;
            if (PrawePodmenu.Children.Count > 0)
            {
                PrawePodmenu.Children.Remove(PrawePodmenu.Children[0]);
            }
            PrawePodmenu.Children.Add(preparePrzegladanieKafelkow());
        }

        private UIElement preparePrzegladanieKafelkow()
        {
            return new PanelPrzegladaniaKafelekView()
                { DataContext = PrzegladanieGrupViewModel.przegladanieGrupViewModel};
        }

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

        private void KontrolaKursowComboItem_OnSelected(object sender, RoutedEventArgs e)
        {
            if (PrawePodmenu == null)
                return;
            if (PrawePodmenu.Children.Count > 0)
                PrawePodmenu.Children.Remove(PrawePodmenu.Children[0]);
            PrawePodmenu.Children.Add(new KontrolaZapisowView());
        }
    }
}
