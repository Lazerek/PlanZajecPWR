using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using PlanZajec.ViewModels;
using PlanZajec.DataModel;

namespace Wpf
{
    /// <summary>
    /// Klasa pozwalająca przeglądać wszystkie grupy
    /// </summary>
    /// 

    public partial class PrzegladanieGrup : UserControl
    {
        long lastLong;
        TextBox tbox;
        private PrzegladanieGrupViewModel viewModel;
        /// <summary>
        /// Domyślny konstuktor przeglądania grup
        /// </summary>
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
                TextBox t = (TextBox)VisualTreeHelper.GetChild(DgUsers.Columns[10].GetCellContent(gz), 0);
                t.Text = gz.Miejsca.Value.ToString();
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
                        CheckBox c = (CheckBox)VisualTreeHelper.GetChild(DgUsers.Columns[9].GetCellContent(gz), 0);
                        c.IsChecked = true;
                    }
                    else
                    {
                        gz.ZajeteMiejsca = gz.Miejsca;
                        viewModel.ZmienLiczbeMiejsc(gz.KodGrupy, (long)gz.Miejsca);
                        CheckBox c = (CheckBox)VisualTreeHelper.GetChild(DgUsers.Columns[9].GetCellContent(gz), 0);
                        c.IsChecked = false;
                    }
                }
            }
            else
            {
                tbox.Text = "0";
                gz.ZajeteMiejsca = 0;
                viewModel.ZmienLiczbeMiejsc(gz.KodGrupy, 0);
                CheckBox c = (CheckBox)VisualTreeHelper.GetChild(DgUsers.Columns[9].GetCellContent(gz), 0);
                c.IsChecked = true;
            }
        }
        private void sprawdzEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox t = (TextBox)sender;
                Keyboard.ClearFocus();
                GrupyZajeciowe gz = (GrupyZajeciowe)DgUsers.CurrentCell.Item;
                long l = 0;
                tbox = (TextBox)sender;
                if (tbox.Text.Length != 0)
                {
                    if (long.TryParse(tbox.Text, out l))
                    {
                        if (l > -1 && l < gz.Miejsca)
                        {
                            gz.ZajeteMiejsca = l;
                            viewModel.ZmienLiczbeMiejsc(gz.KodGrupy, l);
                            CheckBox c = (CheckBox)VisualTreeHelper.GetChild(DgUsers.Columns[9].GetCellContent(gz), 0);
                            c.IsChecked = true;
                        }
                        else
                        {
                            gz.ZajeteMiejsca = gz.Miejsca;
                            viewModel.ZmienLiczbeMiejsc(gz.KodGrupy, (long)gz.Miejsca);
                            CheckBox c = (CheckBox)VisualTreeHelper.GetChild(DgUsers.Columns[9].GetCellContent(gz), 0);
                            c.IsChecked = false;
                        }
                    }
                }
                else
                {
                    tbox.Text = "0";
                    gz.ZajeteMiejsca = 0;
                    viewModel.ZmienLiczbeMiejsc(gz.KodGrupy, 0);
                    CheckBox c = (CheckBox)VisualTreeHelper.GetChild(DgUsers.Columns[9].GetCellContent(gz), 0);
                    c.IsChecked = true;
                }
                e.Handled = true;
            }
        }
    }
}
