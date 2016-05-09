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
using System.Windows.Shapes;

namespace PlanZajec.Views
{
    /// <summary>
    /// Interaction logic for AddPlanWindow.xaml
    /// </summary>
    public partial class AddPlanWindow : Window
    {
        public AddPlanWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Dodaj(object sender, RoutedEventArgs e)
        {
            if(PlanTitle == null || PlanTitle.Length == 0)
            {
                MessageBox.Show("Plan musi mieć nazwę!");
            }
            else
            {
                DialogResult = true;
            }
        }

        private void Anuluj(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public string PlanTitle { get; set; }
    }
}
