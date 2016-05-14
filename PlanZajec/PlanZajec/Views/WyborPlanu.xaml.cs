using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
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

namespace PlanZajec.Views
{
    /// <summary>
    /// Interaction logic for WyborPlanu.xaml
    /// </summary>
    public partial class WyborPlanu : UserControl
    {
        public event Action<Plany> ChosenPlanToShowEventHandler;
        public event Func<Plany,bool> ChosenPlanToDeleteEventHandler;

        public event Action<string> AddToPlan;

        public WyborPlanu()
        {
            InitializeComponent();
        }

        private void ZmienKolorMouseEnter(object sender, MouseEventArgs e)
        {
            KafelPlanu plan = sender as KafelPlanu;
            plan.WypelnieniePlansza.Background = new SolidColorBrush(Colors.GreenYellow);
            
        }

        private void ZmienKolorMouseLeave(object sender, MouseEventArgs e)
        {
            KafelPlanu plan = sender as KafelPlanu;
            plan.WypelnieniePlansza.Background = new SolidColorBrush(Colors.LightBlue);
        }

        private void ButtonPlanMouseEnter(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            button.Background = new SolidColorBrush(Colors.Transparent);
        }


        private void WywierzPlan(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.Write("Wybor planu");
            Button button = sender as Button;
            button.Background = new SolidColorBrush(Colors.Transparent);
            ChosenPlanToShowEventHandler?.Invoke(button.DataContext as Plany);
        }

        private void Usun(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.Write("usuwanie plany");
            Button button = sender as Button;
            if (ChosenPlanToDeleteEventHandler != null)
            {
                bool deleted = ChosenPlanToDeleteEventHandler(button.DataContext as Plany);
            }
        }

        private void DodajPlan(object sender, RoutedEventArgs e)
        {
            AddPlanWindow addPlan = new AddPlanWindow();
            bool? result = addPlan.ShowDialog();
            if (result.HasValue && result.Value)
            {
                AddToPlan?.Invoke(addPlan.PlanTitle);
            }
        }
    }
}
