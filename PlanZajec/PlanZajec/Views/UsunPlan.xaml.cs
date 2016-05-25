using PlanZajec.DataModel;
using PlanZajec.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PlanZajec
{
    /// <summary>
    /// Interaction logic for UsunPlan.xaml
    /// </summary>
    public partial class UsunPlan : UserControl
    {
    public UsunPlan()
        {
            InitializeComponent();
            this.DataContext = UsunPlanViewModel.Instance;
        }

        private void Usun(object sender, RoutedEventArgs e)
        {
            if(listaPlanow.Items.Count > 1)
            {
                var plan = (Plany)listaPlanow.SelectedItem;
                UsunPlanViewModel.Instance.UsunPlan(plan);
                WyborPlanuViewModel.Instance.UsunPlan(plan);
            }
            else
            {
                MessageBox.Show("Nie można usunąć ostatniego istniejącego planu.", "Błąd");
            }
        }
    }
}
