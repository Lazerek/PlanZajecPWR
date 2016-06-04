using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using PlanZajec.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace PlanZajec
{
    /// <summary>
    /// Klasa typu window do usuwania planów
    /// </summary>
    public partial class UsunPlan : UserControl
    {
        /// <summary>
        /// Domyslny konstruktor zawierający instancje planów
        /// </summary>
    public UsunPlan()
        {
            InitializeComponent();
            this.DataContext = PlanyViewModel.Instance;
        }
        /// <summary>
        /// Metoda usuwająca plan
        /// </summary>

        private void Usun(object sender, RoutedEventArgs e)
        {
            var plan = (Plany)listaPlanow.SelectedItem;
            using (var unit = new UnitOfWork(new PlanPwrContext()))
            {
                unit.Plany.Remove(plan);
                unit.SaveChanges();
            }
            PlanyViewModel.Instance.UsunPlan(plan);
        }
    }
}
