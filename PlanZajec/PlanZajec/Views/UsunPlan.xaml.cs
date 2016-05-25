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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Usun(object sender, RoutedEventArgs e)
        {
            if(listaPlanow.Items.Count > 1)
            {
                var plan = (Plany)listaPlanow.SelectedItem;
                PlanyViewModel.Instance.UsunPlan(plan);
            }
            else
            {
                MessageBox.Show("Nie można usunąć ostatniego istniejącego planu.", "Błąd");
            }
        }
    }
}
