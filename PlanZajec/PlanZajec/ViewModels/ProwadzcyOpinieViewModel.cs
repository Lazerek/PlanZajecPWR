using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;

namespace PlanZajec.ViewModels
{
    /// <summary>
    /// Klasa typu ViewModel pomagająca wyświetlać prowadzących, opinie oraz oceny o nich
    /// </summary>
    public class ProwadzacyOpinieViewModel : ViewModel
    {
        public List<Prowadzacy> Items { get; set; }
        public List<Prowadzacy> ComboBoxItems { get; set; }
        /// <summary>
        /// Domyślny konstrkutor pobierający prowadzących do listy przedmiotów
        /// </summary>
        public ProwadzacyOpinieViewModel()
        {
            ComboBoxItems = new List<Prowadzacy>();
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                ComboBoxItems = uw.Prowadzacy.GetAll().ToList();
            }
        }
        /// <summary>
        /// Metoda pozwalająca pobrać opinię oraz ocenę o prowadzącym
        /// </summary>
        /// <param name="pr">Prowadzący</param>
        /// <returns>Opinia oraz ocena prowadzącego</returns>
        public string[] dajOpinie(string pr)
        {
            String[] wynik = new string[2];
            
            int count = 0;
            for (int i = 0; i < pr.Length; i++)
            {
                if (pr[i].Equals(' '))
                    count++;
            }
            var tytulNazwiskoImie = pr.Split(' ');
            count = 0;
            foreach (string s in tytulNazwiskoImie)
            {
                count++;
            }
            String Nazwisko = tytulNazwiskoImie[count - 1];
            String Imie = tytulNazwiskoImie[count - 2];
            int j = 0;
            for (int i = pr.Length - 1; i > Nazwisko.Length + Imie.Length; i--)
            {
                j++;
            }
            String Tytul = pr.Substring(0, j - 1);
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                var prow = uw.Prowadzacy.GetAll().ToList();

                foreach (Prowadzacy p in prow)
                {
                    if(p.Nazwisko.Equals(Nazwisko)&&p.Imie.Equals(Imie)&&p.Tytul.Equals(Tytul))
                    {
                        wynik[0] = p.Opis;
                        wynik[1] = p.Ocena + "";
                    }
                }
            }
            return wynik;
        }
        /// <summary>
        /// Metoda pozwalająca zapisać ocenę i opinię prowadzącego
        /// </summary>
        /// <param name="pr">Dane prowadzącego</param>
        /// <param name="index">Indeks prowadzącego</param>
        /// <param name="opinia">Opinia o prowadzącym</param>
        /// <param name="ocena">Ocena o prowadzącym</param>
        /// <returns>Poprawność zapisu opinii i oceny</returns>
        public Boolean ZapiszOpinie(string pr, int index, string opinia, string ocena)
        {
            Boolean returnValue = false;
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                var prow = uw.Prowadzacy.GetAll().ToList();
                foreach (Prowadzacy p in prow)
                {
                    System.Diagnostics.Debug.WriteLine(p.IdProwadzacego + " " + index);
                    if (p.IdProwadzacego == index)
                    {
                        System.Diagnostics.Debug.WriteLine(p.IdProwadzacego + " " + p.Nazwisko + " " + index + " " + opinia);
                        p.Opis = opinia;
                        double ocenaDoZapisu;
                        ocena.Replace('.', ',');
                        if (double.TryParse(ocena, out ocenaDoZapisu))
                        {
                            if (ocenaDoZapisu >= 2.0f && ocenaDoZapisu <= 5.5f)
                                p.Ocena = double.Parse(ocena);
                            else
                                returnValue = true;
                        }             
                    }
                }
                uw.SaveChanges();
            }
            return returnValue;
        }
    }
}

