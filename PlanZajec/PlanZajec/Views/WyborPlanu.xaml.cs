using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using PlanZajec.ViewModels;
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

        private void DodajAlternatywnyPlan(object sender, RoutedEventArgs e)
        {
            ListaPlanow lp = preparePlanList();
            lp.Show();
        }

        private ListaPlanow preparePlanList()
        {
            var res = new ListaPlanow();
            res.DodajPlan += AddPlan;
            return res;
        }

        private void AddPlan(string Title, int id)
        {
            Plany plan;
            using (var unit = new UnitOfWork(new PlanPwrContext()))
            {
                plan = new Plany { NazwaPlanu = Title };
                var plist = unit.Plany.GetAll().GetEnumerator();
                int i = 0;
                while (plist.MoveNext())
                {
                    if (id == i)
                    {
                        foreach (GrupyZajeciowe g in plist.Current.GrupyZajeciowe)
                        {
                            plan.GrupyZajeciowe.Add(g);
                        }
                    }
                    else
                    {
                        i++;
                    }
                }
                unit.Plany.Add(plan);
                unit.SaveChanges();
                WyborPlanuViewModel.Instance.DodajPlan(plan);
                UsunPlanViewModel.Instance.DodajPlan(plan);
            }
        }
    }
}
