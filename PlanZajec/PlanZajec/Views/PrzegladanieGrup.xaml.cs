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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PlanZajec.ViewModels;
using PlanZajec.DataModel;
using System.Text.RegularExpressions;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for PrzegladanieGrup.xaml
    /// </summary>
    /// 

    public partial class PrzegladanieGrup : UserControl
    {

        TextBox tbox;
        private PrzegladanieGrupViewModel viewModel;
        public PrzegladanieGrup()
        {
            InitializeComponent();
            viewModel = new PrzegladanieGrupViewModel();
            this.DataContext = viewModel;
        }
        void onCheckBoxClick(object sender, RoutedEventArgs e)
        {
            GrupyZajeciowe gz = (GrupyZajeciowe)DgUsers.CurrentCell.Item;
            if (gz.ZajeteMiejsca < gz.Miejsca){
                gz.ZajeteMiejsca = gz.Miejsca;
                viewModel.ZmienLiczbeMiejsc(gz.KodGrupy, (long)gz.Miejsca);
            }else{
                gz.ZajeteMiejsca = gz.Miejsca - 1;
                viewModel.ZmienLiczbeMiejsc(gz.KodGrupy, (long)gz.Miejsca - 1);
            }
            DgUsers.Items.Refresh();
        }
        void checkText(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        void onTextboxLeave(object sender, RoutedEventArgs e)
        {
            GrupyZajeciowe gz = (GrupyZajeciowe)DgUsers.CurrentCell.Item;
            long l = 0;
            tbox = (TextBox)sender;
            if (long.TryParse(tbox.Text, out l))
            {
                gz.ZajeteMiejsca = l;
                viewModel.ZmienLiczbeMiejsc(gz.KodGrupy, l);
            }
        }
    }
}
