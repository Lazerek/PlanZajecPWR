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

namespace Wpf
{
    /// <summary>
    /// Interaction logic for PraweMenu.xaml
    /// </summary>
    public partial class PraweMenu : UserControl
    {
        bool lpm;
        OknoOpcji oo;
        public PraweMenu()
        {
            InitializeComponent();
            lpm = false;
            oo = new OknoOpcji();
            loadOptImg();
            PrawePodmenu.Children.Add(new Grupa());
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

        private void OnSelectedGroups(object sender, RoutedEventArgs e)
        {
            if (PrawePodmenu == null) return;
            if (PrawePodmenu.Children.Count > 0)
            {
                PrawePodmenu.Children.Remove(PrawePodmenu.Children[0]);
            }
            PrawePodmenu.Children.Add(new Grupa());
        }

        public void loadOptImg()
        {
            optImg.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "..\\..\\..\\Views\\Images\\gear.png"));
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
    }
}
