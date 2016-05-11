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
        long lastLong;
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
            CheckBox c = (CheckBox)sender;
            if (!c.IsChecked.Value)
            {
                gz.ZajeteMiejsca = gz.Miejsca.Value;
                viewModel.ZmienLiczbeMiejsc(gz.KodGrupy, gz.Miejsca.Value);
                DgUsers.Items.Refresh();
            }
            else
            {
                c.IsChecked = false;
            }
        }

        private void gotFocus(object sender, RoutedEventArgs e)
        {
            TextBox t = (TextBox)sender;
            lastLong = 0;
            long.TryParse(t.Text, out lastLong);
        }

            void textChanged(object sender, RoutedEventArgs e)
        {
            TextBox t = (TextBox)sender;
            string sCheck = t.Text;
            int selection = t.SelectionStart;
            int preLength = sCheck.Length;
            if (sCheck.Length > 1)
            {
                sCheck = sCheck.TrimStart('0');
                for (int i = preLength; i > sCheck.Length; i--)
                {
                    selection--;
                }
            }
            if (sCheck.Length > 3)
            {
                sCheck = lastLong.ToString();
                selection--;
            }
            for(int i = 0; i < sCheck.Length; i++)
            {
                if((int)sCheck[i] < 48 || (int)sCheck[i] > 57)
                {
                    sCheck = sCheck.Remove(i);
                }
            }
            t.Text = sCheck;
            t.SelectionStart = selection;
            long.TryParse(t.Text, out lastLong);
        }
        void checkText(object sender, TextCompositionEventArgs e)
        {
            long l = 0;
            e.Handled = !long.TryParse(e.Text, out l);
        }

        
        void onTextboxLeave(object sender, RoutedEventArgs e)
        {
            GrupyZajeciowe gz = (GrupyZajeciowe)DgUsers.CurrentCell.Item;
            long l = 0;
            tbox = (TextBox)sender;
            if(tbox.Text.Length != 0)
            {
                if (long.TryParse(tbox.Text, out l))
                {
                    if(l > -1 && l < gz.Miejsca)
                    {
                        gz.ZajeteMiejsca = l;
                        viewModel.ZmienLiczbeMiejsc(gz.KodGrupy, l);
                    }
                    else
                    {
                        gz.ZajeteMiejsca = gz.Miejsca;
                        viewModel.ZmienLiczbeMiejsc(gz.KodGrupy, (long)gz.Miejsca);
                    }
                }
            }
            else
            {
                tbox.Text = "0";
                gz.ZajeteMiejsca = 0;
                viewModel.ZmienLiczbeMiejsc(gz.KodGrupy, 0);
            }
        }
    }
}
