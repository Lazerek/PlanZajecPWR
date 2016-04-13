using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
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
    /// Interaction logic for EdycjaLiczbyMiejsc.xaml
    /// </summary>
    public partial class EdycjaLiczbyMiejsc : Window
    {
        string kodGrupy;
        public EdycjaLiczbyMiejsc()
        {
            InitializeComponent();
        }

        public EdycjaLiczbyMiejsc(string kodGrupy)
        {
            InitializeComponent();
            this.kodGrupy = kodGrupy;
        }

        public void Anuluj()
        {
            Close();
        }

        public void Ok(object sender)
        {
            long liczbaMiejsc = 0;
            if(Int64.TryParse(tb.Text, out liczbaMiejsc))
            {
                using (var uw = new UnitOfWork(new PlanPwrContext()))
                {
                    int exist = -1;
                    List<GrupyZajeciowe> k = uw.GrupyZajeciowe.GetAll().ToList();
                    for (int i = 0; i < k.Count; i++)
                    {
                        if (k[i].KodGrupy == kodGrupy)
                        {
                            exist = i;
                        }
                    }
                    if (exist != -1)
                    {
                        k[exist].Miejsca = liczbaMiejsc;
                    }
                    else
                    {
                        MessageBox.Show("Nie znaleziono grupy.", "Błąd");
                    }
                }
            }
            else
            {
                MessageBox.Show("Liczba miejsc musi być liczbą.", "Błąd");
            }
        }
    }
}
