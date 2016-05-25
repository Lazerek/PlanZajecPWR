using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using PlanZajec.ViewModels;
using System.Windows.Media;

namespace PlanZajec.Views
{
    /// <summary>
    /// Klasa do ustawiania wolnych dni i godzin
    /// </summary>
    public partial class WolneDniSetView : UserControl
    {
        /// <summary>
        /// Domyślny konstruktor pobierający plany
        /// </summary>
        public WolneDniSetView()
        {
            InitializeComponent();
            this.DataContext = PlanyViewModel.Instance;
            /*
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                plany = new ObservableCollection<Plany>(uw.Plany.GetAll().ToList());
            }

            foreach (var plan in plany)
            {
                SelectPlanComboBox.Items.Add(plan.NazwaPlanu);
            }
            */
        }

        public ObservableCollection<Plany> plany { get; }

        /// <summary>
        /// Metoda dodająca wolne przydziały do planu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DodajWolneButton_OnClick(object sender, RoutedEventArgs e)
        {
            if(SelectPlanComboBox.SelectedItem != null)
            {
                var plan = (Plany)SelectPlanComboBox.SelectedItem;
                using (var unit = new UnitOfWork(new PlanPwrContext()))
                {
                    Plany zmienianyPlan = unit.Plany.Get(plan.IdPlanu);
                    zmienianyPlan.AddWolneDni(getBeaginingHour() + ":" + getEndHour() + ":" + getShortDay());
                }
                Info.Text = "Dodano wolne godziny do planu";
                Info.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }
            else
            {
                Info.Text = "Nie wybrano planu";
                Info.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
        /// <summary>
        /// Pobranie zaznaczonego planu
        /// </summary>
        /// <param name="name">Nazwa planu</param>
        /// <returns>Plan</returns>
        private Plany getSelectedPlan(string name)
        {
            return plany.FirstOrDefault(plan => plan.NazwaPlanu == name);
        }
        /// <summary>
        /// Pobranie godzin początkowych
        /// </summary>
        /// <returns>Godzinępoczątkową</returns>
        private string getBeaginingHour()
        {
            var beginingHour = "";
            switch (GodzinaRozpoczeciaComboBox.Text)
            {
                case "7.30":
                    beginingHour = "07";
                    break;
                case "9.15":
                    beginingHour = "09";
                    break;
                case "11.15":
                    beginingHour = "11";
                    break;
                case "13.15":
                    beginingHour = "13";
                    break;
                case "15.15":
                    beginingHour = "15";
                    break;
                case "17.05":
                    beginingHour = "17";
                    break;
                case "18.55":
                    beginingHour = "19";
                    break;
            }
            return beginingHour;
        }
        /// <summary>
        /// Pobranie godziny końcowej
        /// </summary>
        /// <returns>Godzina końcowa</returns>
        private string getEndHour()
        {
            var endHour = "";
            switch (GodzinaZakonczeniaComboBox.Text)
            {
                case "9.00":
                    endHour = "09";
                    break;
                case "11.00":
                    endHour = "11";
                    break;
                case "13.00":
                    endHour = "13";
                    break;
                case "15.00":
                    endHour = "15";
                    break;
                case "16.55":
                    endHour = "17";
                    break;
                case "18.45":
                    endHour = "19";
                    break;
                case "20.35":
                    endHour = "21";
                    break;
            }
            return endHour;
        }
        /// <summary>
        /// Pobranie dnia
        /// </summary>
        /// <returns>Skrócona nazwa dnia</returns>
        private string getShortDay()
        {
            var dayShort = "";
            switch (DaySelectComboBox.SelectedIndex)
            {
                case 0:
                    dayShort = "pn";
                    break;
                case 1:
                    dayShort = "wt";
                    break;
                case 2:
                    dayShort = "śr";
                    break;
                case 3:
                    dayShort = "cz";
                    break;
                case 4:
                    dayShort = "pt";
                    break;
                case 5:
                    dayShort = "sb";
                    break;
                case 6:
                    dayShort = "nd";
                    break;
            }
            return dayShort;
        }
    }
}