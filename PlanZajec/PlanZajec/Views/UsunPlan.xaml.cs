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
        private UsunPlanViewModel ViewModel;
        private WyborPlanuViewModel ViewModelG;
    public UsunPlan()
        {
            InitializeComponent();
            ViewModel = new UsunPlanViewModel();
            this.DataContext = ViewModel;
        }

        private void Usun(object sender, RoutedEventArgs e)
        {
            var c = (Plany)listaPlanow.SelectedItem;
            ViewModel.usunPlan(c);
        }
    }
}
