using PlanZajec.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PlanZajec.DataModel;
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
    /// Interaction logic for ListaPlanow.xaml
    /// </summary>
    public partial class ListaPlanow : Window
    {
        public event Action<string, int> DodajPlan;

        public int id;
        private ListaPlanowViewModel viewModel;

        public ListaPlanow()
        {
            InitializeComponent();
            viewModel = new ListaPlanowViewModel();
            this.DataContext = viewModel;
        }
   
        public void Anuluj(object sender, EventArgs e)
        {
            this.Close();
        }

        public void Utworz(object sender, EventArgs e)
        {
            if(plList.SelectedItems.Count > 0)
            {
                if (!Nazwa.Text.Equals(""))
                {
                    id = plList.SelectedIndex;
                    DodajPlan?.Invoke(Nazwa.Text, id);
                    MessageBox.Show("Utworzono nowy alternatywny plan.", "Dodano plan");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Nie wybrano nazwy dla nowego planu.", "Brak nazwy planu");
                }
            }
            else
            {
                MessageBox.Show("Nie wybrano podstawowego planu.", "Nie wybrano planu");
            }
        }
    }
}
