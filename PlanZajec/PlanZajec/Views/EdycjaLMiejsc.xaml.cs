using PlanZajec.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PlanZajec.Views
{
    /// <summary>
    /// Interaction logic for EdycjaLMiejsc.xaml
    /// </summary>
    public partial class EdycjaLMiejsc : Window
    {
        string kodGrupy;
        public EdycjaLMiejsc()
        {
            InitializeComponent();
            
        }

        public EdycjaLMiejsc(string kodGrupy)
        {
            InitializeComponent();
            this.kodGrupy = kodGrupy;
            
        }

        public void Anuluj(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void Ok(object sender, RoutedEventArgs e)
        {
            using (var uw = new UnitOfWork(new DataModel.PlanPwrContext()))
            {
                List<DataModel.GrupyZajeciowe> gz = uw.GrupyZajeciowe.GetAll().ToList();
                long nowaLiczbaMiejsc = 0;
                if(Int64.TryParse(lm.Text, out nowaLiczbaMiejsc))
                {
                    for (int i = 0; i < gz.Count; i++)
                    {
                        if (gz[i].KodGrupy == kodGrupy)
                        {
                            gz[i].Miejsca = nowaLiczbaMiejsc;
                        }
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Liczba miejsc musi być liczbą całkowitą.", "Błąd", MessageBoxButtons.OK,  MessageBoxIcon.Exclamation,  MessageBoxDefaultButton.Button1);
                }
            }
        }
    }
}
