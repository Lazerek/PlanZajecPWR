using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace PlanZajec.Views
{
    /// <summary>
    /// Klasa pozwalająca na edycję liczby miejsc
    /// </summary>
    public partial class EdycjaLiczbyMiejsc : Window
    {
        string kodGrupy;
        /// <summary>
        /// Domyślny konstruktor
        /// </summary>
        public EdycjaLiczbyMiejsc()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Konstruktor z kodem grupy
        /// </summary>
        /// <param name="kodGrupy">Kod Grupy</param>
        public EdycjaLiczbyMiejsc(string kodGrupy)
        {
            InitializeComponent();
            this.kodGrupy = kodGrupy;
        }
        /// <summary>
        /// Metoda pozwalająca anulować zmianę
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Anuluj(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// Metoda zmieniająca liczbę wolnych miejsc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Ok(object sender, EventArgs e)
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
                        k[exist].ZajeteMiejsca = liczbaMiejsc;
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
