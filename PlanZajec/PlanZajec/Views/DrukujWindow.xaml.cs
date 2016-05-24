using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using PrintDialog = System.Windows.Controls.PrintDialog;
using PlanZajec.ViewModels;

namespace PlanZajec.Views
{
    /// <summary>
    /// Interaction logic for DrukujWindow.xaml
    /// </summary>
    public partial class DrukujWindow : Window
    {
        public ObservableCollection<Plany> plany { get; private set; }
        private long[] tab;
        public DrukujWindow()
        {
            InitializeComponent();
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                plany = new ObservableCollection<Plany>(uw.Plany.GetAll().ToList());
            }
            tab = new long[plany.Count()];
            int i = 0;
            foreach (Plany plan in plany)
            {
                PlanyComboBox.Items.Add(plan.NazwaPlanu);
                tab[i] = plan.IdPlanu;
                i++;
            }
            PlanyComboBox.SelectedIndex = 0;
        }

        private void DrukujButton_Click(object sender, RoutedEventArgs e)
        {
            PlanViewModel pvm = new PlanViewModel(tab[PlanyComboBox.SelectedIndex]);
            PlanView pv = new PlanView(pvm);
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(pv, "Plan");
            }
        }
    }
}
