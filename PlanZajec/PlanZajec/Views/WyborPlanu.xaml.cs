using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using PlanZajec.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PlanZajec.Views
{
    /// <summary>
    /// Klasa pozwalajaca wybrać plan
    /// </summary>
    public partial class WyborPlanu : UserControl
    {
        public event Action<Plany> ChosenPlanToShowEventHandler;
        public event Func<Plany,bool> ChosenPlanToDeleteEventHandler;
        /// <summary>
        /// Domyślny konstruktor
        /// </summary>
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

        /// <summary>
        /// Metoda pomagająca wybrać plan
        /// </summary>

        private void WywierzPlan(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.Write("Wybor planu");
            Button button = sender as Button;
            button.Background = new SolidColorBrush(Colors.Transparent);
            ChosenPlanToShowEventHandler?.Invoke(button.DataContext as Plany);
        }
        /// <summary>
        /// Metoda usuwania planu
        /// </summary>

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
        /// <summary>
        /// Metoda dodania planu
        /// </summary>

        private void DodajPlan(object sender, RoutedEventArgs e)
        {
            AddPlanWindow addPlan = new AddPlanWindow();
            addPlan.ShowDialog();
        }
        /// <summary>
        /// Metoda dodania planu alternatywnego
        /// </summary>

        private void DodajAlternatywnyPlan(object sender, RoutedEventArgs e)
        {
            ListaPlanow lp = preparePlanList();
            lp.ShowDialog();
        }
        /// <summary>
        /// Metoda przygotowująca listę planów
        /// </summary>
        /// <returns>Lista planów</returns>
        private ListaPlanow preparePlanList()
        {
            var res = new ListaPlanow();
            res.DodajPlan += AddPlan;
            return res;
        }
        /// <summary>
        /// Metoda dodająca plan
        /// </summary>
        /// <param name="Title">Nazwa planu</param>
        /// <param name="plan">Pierwotny plan</param>
        private void AddPlan(string Title, Plany plan)
        {
            using (var unit = new UnitOfWork(new PlanPwrContext()))
            {
                Plany nowyPlan = new Plany { NazwaPlanu = Title };
                var staryPlan = unit.Plany.Get(plan.IdPlanu);
                foreach (GrupyZajeciowe g in staryPlan.GrupyZajeciowe)
                {
                    nowyPlan.GrupyZajeciowe.Add(g);
                }
                unit.Plany.Add(nowyPlan);
                unit.SaveChanges();
                PlanyViewModel.Instance.DodajPlan(nowyPlan);
            }
        }
    }
}
