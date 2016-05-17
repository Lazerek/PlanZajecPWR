using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using PlanZajec.Parser;
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
using System.Windows.Shapes;

namespace PlanZajec.Views
{
    /// <summary>
    /// Interaction logic for ZapisWindow.xaml
    /// </summary>

    
    public partial class ZapisWindow : Window
    {
        public ObservableCollection<Plany> plany { get; private set; }
        private long[] tab;
        public ZapisWindow()
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
                PlanyComboBox.Items.Add("Plan " + plan.IdPlanu);
                tab[i] = plan.IdPlanu;
            }
            PlanyComboBox.SelectedIndex = 0;
        }

        private void Zapisz_Click(object sender, RoutedEventArgs e)
        {
            int index = PlanyComboBox.SelectedIndex;
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "Plan zajęć (*.txt)|*.txt"
            };
            if (dialog.ShowDialog() == true)
            {
                //ZapisDoBazy.export(dialog, tab[index]);
                System.Diagnostics.Debug.WriteLine(tab[index]);
            }
        }
    }

}
