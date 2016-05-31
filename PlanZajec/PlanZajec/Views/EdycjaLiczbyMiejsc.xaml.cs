﻿using PlanZajec.DataAccessLayer;
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
            if (!Int64.TryParse(tb.Text, out liczbaMiejsc))
            {
                MessageBox.Show("Liczba miejsc musi być liczbą.", "Błąd");
                return;
            }
            if (!SaveNumberOfFreePlaces(liczbaMiejsc))
            {
                MessageBox.Show("Nie znaleziono grupy.", "Błąd");
            }
        }

        private bool SaveNumberOfFreePlaces(long liczbaMiejsc)
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                GrupyZajeciowe grupa = uw.GrupyZajeciowe.Get(kodGrupy);
                if (grupa != null)
                {
                    grupa.ZajeteMiejsca = liczbaMiejsc;
                    uw.SaveChanges();
                    return true;
                }
            }
            return false;
        }
    }
}
