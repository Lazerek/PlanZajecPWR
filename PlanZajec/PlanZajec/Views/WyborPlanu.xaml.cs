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
        public delegate void ChosenPlan(Plany plan);

        public delegate void ChosenPlan2(Plany plan, out bool deleted);

        public event ChosenPlan ChosenPlanToShowEventHandler;
        public event ChosenPlan2 ChosenPlanToDeleteEventHandler;

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
            if(ChosenPlanToShowEventHandler != null)
            {
                ChosenPlanToShowEventHandler(button.DataContext as Plany);
            }
        }

        private void Usun(object sender, RoutedEventArgs e)
        {
            var numberOfSch = PlanyKafleControl.Items.Count;
            if (numberOfSch == 1)
            {
                MessageBox.Show("Nie można usunąć jedynego planu");
                return;
            }
            System.Diagnostics.Debug.Write("usuwanie plany");
            Button button = sender as Button;
            if (ChosenPlanToDeleteEventHandler != null)
            {
                bool deleted=false;
                ChosenPlanToDeleteEventHandler(button.DataContext as Plany, out deleted);
            }
        }

        private void DodajPlan(object sender, RoutedEventArgs e)
        {
            AddPlanWindow addPlan = new AddPlanWindow();
            bool? result = addPlan.ShowDialog();
            if (result.HasValue && result.Value)
            {

                if (AddToPlan != null)
                {
                    AddToPlan(addPlan.PlanTitle);
                }



            }
        }
    }
}
