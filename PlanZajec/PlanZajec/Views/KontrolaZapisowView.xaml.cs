using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using PlanZajec.ViewModels;
using System.Windows;
using System.Windows.Data;
using System;
using System.Collections.ObjectModel;

namespace PlanZajec.Views
{
    /// <summary>
    /// Klasa odpowiedzialna za kontrolę zapisów
    /// </summary>
    public partial class KontrolaZapisowView : UserControl
    {

        private PrzegladanieGrupViewModel przegladanieViewModel;
        private List<string> listaKursow;
        private List<GrupyZajeciowe> gZaj;
        private int numerWczytywanego;

        /// <summary>
        /// Domyślny konstruktor, sprawdza poprawność zapisu
        /// </summary>
        public KontrolaZapisowView()
        {
            InitializeComponent();
            this.DataContext = PlanyViewModel.Instance;
            przegladanieViewModel = new PrzegladanieGrupViewModel();
            listaKursow = new List<string>();
            numerWczytywanego = 0;
            utworzListeKursow(listaKursow);
            listaKursow.Sort();
        }

        /// <summary>
        /// Metoda obsługująca utworzenie listy wszystkich z bazy kursów
        /// </summary>
        private void utworzListeKursow(List<string> lk)
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                gZaj = uw.GrupyZajeciowe.GetAll().ToList();
                foreach (GrupyZajeciowe g in gZaj)
                {
                    switch (g.TypZajec)
                    {
                        case "Zajęcia laboratoryjne":
                            if (!lk.Contains(g.Kursy.NazwaKursu + " L"))
                            {
                                lk.Add(g.Kursy.NazwaKursu + " L");
                            }
                        break;
                        case "Wykład":
                            if (!lk.Contains(g.Kursy.NazwaKursu + " W"))
                            {
                                lk.Add(g.Kursy.NazwaKursu + " W");
                            }
                        break;
                        case "Projekt":
                            if (!lk.Contains(g.Kursy.NazwaKursu + " P"))
                            {
                                lk.Add(g.Kursy.NazwaKursu + " P");
                            }
                        break;
                        case "Ćwiczenia":
                            if (!lk.Contains(g.Kursy.NazwaKursu + " Ć"))
                            {
                                lk.Add(g.Kursy.NazwaKursu + " Ć");
                            }
                        break;
                        case "Seminarium":
                            if (!lk.Contains(g.Kursy.NazwaKursu + " S"))
                            {
                                lk.Add(g.Kursy.NazwaKursu + " S");
                            }
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Metoda oznaczająca kursy na które zapisał się słuchacz
        /// </summary>
        private void wypelnijListe(object sender, EventArgs e)
        {
            StackPanel sp = (StackPanel)sender;
            Plany p = (Plany)items.Items.GetItemAt(numerWczytywanego);
            List<string> listaZapisanych = new List<string>();
            foreach (GrupyZajeciowe g in p.GrupyZajeciowe)
            {
                var enumerator = gZaj.GetEnumerator();
                enumerator.MoveNext();
                while (enumerator.Current.KodGrupy != g.KodGrupy)
                {
                    enumerator.MoveNext();
                }
                switch(g.TypZajec){
                    case "Zajęcia laboratoryjne":
                        if (!listaZapisanych.Contains(enumerator.Current.Kursy.NazwaKursu + " L"))
                        {
                            listaZapisanych.Add(enumerator.Current.Kursy.NazwaKursu + " L");
                        }
                        break;
                    case "Wykład":
                        if (!listaZapisanych.Contains(enumerator.Current.Kursy.NazwaKursu + " W"))
                        {
                            listaZapisanych.Add(enumerator.Current.Kursy.NazwaKursu + " W");
                        }
                        break;
                    case "Projekt":
                        if (!listaZapisanych.Contains(enumerator.Current.Kursy.NazwaKursu + " P"))
                        {
                            listaZapisanych.Add(enumerator.Current.Kursy.NazwaKursu + " P");
                        }
                        break;
                    case "Ćwiczenia":
                        if (!listaZapisanych.Contains(enumerator.Current.Kursy.NazwaKursu + " Ć"))
                        {
                            listaZapisanych.Add(enumerator.Current.Kursy.NazwaKursu + " Ć");
                        }
                        break;
                    case "Seminarium":
                        if (!listaZapisanych.Contains(enumerator.Current.Kursy.NazwaKursu + " S"))
                        {
                            listaZapisanych.Add(enumerator.Current.Kursy.NazwaKursu + " S");
                        }
                        break;
                }
            }
            foreach (string s in listaKursow)
            {
                TextBlock t = new TextBlock();
                t.Text = "  " + s;
                if (listaZapisanych.Contains(s))
                {
                    t.Foreground = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    t.Foreground = new SolidColorBrush(Colors.Red);
                }
                sp.Children.Add(t);
            }
            numerWczytywanego++;
        }

        /// <summary>
        /// Metoda obsługująca wyświetlanie oraz ukrywanie grup zajęciowych planu
        /// </summary>
        private void otworzGrupy(object sender, RoutedEventArgs e)
        {
            TextBlock t = (TextBlock)sender;
            if (t.Text[1] == '▼')
            {
                Binding b = new Binding("NazwaPlanu");
                b.StringFormat = " ▲ {0}";
                t.SetBinding(TextBlock.TextProperty, b);
                Grid g = (Grid)VisualTreeHelper.GetParent(t);
                g.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Auto);
            }
            else
            {
                Binding b = new Binding("NazwaPlanu");
                b.StringFormat = " ▼ {0}";
                t.SetBinding(TextBlock.TextProperty, b);
                Grid g = (Grid)VisualTreeHelper.GetParent(t);
                g.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Pixel);
            }
        }
    }
}
