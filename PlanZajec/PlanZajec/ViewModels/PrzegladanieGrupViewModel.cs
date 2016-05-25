using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;

namespace PlanZajec.ViewModels
{
    public class PrzegladanieGrupViewModel : ViewModel, INotifyPropertyChanged
    {
        public static PrzegladanieGrupViewModel przegladanieGrupViewModel;
        public static PrzegladanieGrupViewModel temp;
        private readonly string cwiczeniaString = "Ćwiczenia";
        private readonly string labString = "Zajęcia laboratoryjne";
        private readonly string projektString = "Projekt";
        private readonly string wykladString = "Wykład";

        public PrzegladanieGrupViewModel()
        {
            przegladanieGrupViewModel = this;
            temp = this;
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                Items =
                    new ObservableCollection<GrupyZajeciowe>(uw.GrupyZajeciowe.GetGrupyZajecioweWithRelations().ToList());
                ItemsNoChange = uw.GrupyZajeciowe.GetGrupyZajecioweWithRelations().ToList();
            }
            ItemsChanged = new List<GrupyZajeciowe>();
        }

        public ObservableCollection<GrupyZajeciowe> Items { get; set; }
        public GrupyZajeciowe SelectedItem { get; set; }

        public List<GrupyZajeciowe> ItemsNoChange { get; set; }

        public List<GrupyZajeciowe> ItemsChanged { get; set; }

        public void reloadData()
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                Items =
                    new ObservableCollection<GrupyZajeciowe>(uw.GrupyZajeciowe.GetGrupyZajecioweWithRelations().ToList());
                ItemsNoChange = uw.GrupyZajeciowe.GetGrupyZajecioweWithRelations().ToList();
            }
        }

        public void czyscFiltrownie()
        {
            Items.Clear();
            Items = new ObservableCollection<GrupyZajeciowe>();
            foreach (var gz in ItemsNoChange)
            {
                Items.Add(gz);
            }
            NotifyPropertyChange("Items");
        }


        public void Filtruj2(string nazwaKursu, string potok, string kodGrupy, string kodKursu, string prowadzacy,
            string wolneMiejsca, bool lab, bool cwiczenia, bool projekt, bool wszystko, bool wyklad, bool wolne)
        {
            long result;
            long.TryParse(wolneMiejsca, out result);
            if (wolne && wszystko)
                FiltrujWolneWszystko(nazwaKursu, potok, kodGrupy, kodKursu, prowadzacy, wolneMiejsca, lab, cwiczenia, projekt, wszystko, wyklad, result);
            else if (wolne && !wszystko)
                FiltrujWolneNieWszystko(nazwaKursu, potok, kodGrupy, kodKursu, prowadzacy, wolneMiejsca, lab, cwiczenia, projekt, wszystko, wyklad, result);
            else if (!wolne && wszystko)
                FiltrujZajeteWszystko(nazwaKursu, potok, kodGrupy, kodKursu, prowadzacy, wolneMiejsca, lab, cwiczenia, projekt, wszystko, wyklad, result);
            else if (!wolne && !wszystko)
                FiltrujZajeteNieWszystko(nazwaKursu, potok, kodGrupy, kodKursu, prowadzacy, wolneMiejsca, lab, cwiczenia, projekt, wszystko, wyklad, result);

            Items.Clear();
            Items = new ObservableCollection<GrupyZajeciowe>();
            foreach (var gz in ItemsChanged)
                Items.Add(gz);
            ItemsChanged.Clear();
            NotifyPropertyChange("Items");
        }

        public void FiltrujWolneWszystko(string nazwaKursu, string potok, string kodGrupy, string kodKursu,
            string prowadzacy, string wolneMiejsca, bool lab, bool cwiczenia, bool projekt, bool wszystko, bool wyklad,
            long result)
        {
            foreach (var gz in ItemsNoChange)
            {
                if (gz.Prowadzacy != null)
                {
                    if (SprawdzGrupeZProwadzacymWolne(nazwaKursu, potok, kodGrupy, kodKursu, prowadzacy, result, gz))
                        ItemsChanged.Add(gz);
                }
                else
                {
                    if (SprawdzGrupeBezProwadzacegoWolne(nazwaKursu, potok, kodGrupy, kodKursu, prowadzacy, result, gz))
                        ItemsChanged.Add(gz);
                }
            }
        }

        public void FiltrujWolneNieWszystko(string nazwaKursu, string potok, string kodGrupy, string kodKursu,
            string prowadzacy, string wolneMiejsca, bool lab, bool cwiczenia, bool projekt, bool wszystko, bool wyklad,
            long result)
        {
            foreach (var gz in ItemsNoChange)
            {
                if (SprawdzTypZajec(lab, cwiczenia, projekt, wyklad, gz))
                {
                    if (gz.Prowadzacy != null)
                    {
                        if (SprawdzGrupeZProwadzacym(nazwaKursu, potok, kodGrupy, kodKursu, prowadzacy, result, gz))
                            ItemsChanged.Add(gz);
                    }
                    else
                    {
                        if (SprawdzGrupeBezProwadzacego(nazwaKursu, potok, kodGrupy, kodKursu, prowadzacy, result, gz))
                            ItemsChanged.Add(gz);
                    }
                }
            }
        }
        public void FiltrujZajeteWszystko(string nazwaKursu, string potok, string kodGrupy, string kodKursu,
            string prowadzacy, string wolneMiejsca, bool lab, bool cwiczenia, bool projekt, bool wszystko, bool wyklad,
            long result)
        {
            foreach (var gz in ItemsNoChange)
            {
                if (gz.Prowadzacy != null)
                {
                    if (SprawdzGrupeZProwadzacym(nazwaKursu, potok, kodGrupy, kodKursu, prowadzacy, result, gz))
                        ItemsChanged.Add(gz);
                }
                else
                {
                    if (SprawdzGrupeBezProwadzacego(nazwaKursu, potok, kodGrupy, kodKursu, prowadzacy, result, gz))
                        ItemsChanged.Add(gz);
                }
            }
        }

        public void FiltrujZajeteNieWszystko(string nazwaKursu, string potok, string kodGrupy, string kodKursu,
            string prowadzacy, string wolneMiejsca, bool lab, bool cwiczenia, bool projekt, bool wszystko, bool wyklad,
            long result)
        {
            foreach (var gz in ItemsNoChange)
            {
                if (SprawdzTypZajec(lab, cwiczenia, projekt, wyklad, gz))
                {
                    if (gz.Prowadzacy != null)
                    {
                        if (SprawdzGrupeZProwadzacym(nazwaKursu, potok, kodGrupy, kodKursu, prowadzacy, result, gz))
                            ItemsChanged.Add(gz);
                    }
                    else
                    {
                        if (SprawdzGrupeBezProwadzacego(nazwaKursu, potok, kodGrupy, kodKursu, prowadzacy, result, gz))
                            ItemsChanged.Add(gz);
                    }
                }
            }
        }
        public Boolean SprawdzGrupeZProwadzacym(string nazwaKursu, string potok, string kodGrupy, string kodKursu, string prowadzacy, long wolneMiejsca, GrupyZajeciowe gz)
        {
            if (SprawdzGrupe(nazwaKursu, potok, kodGrupy, kodGrupy, gz) &&
                            (gz.Prowadzacy.Nazwisko.IndexOf(prowadzacy, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                             gz.Prowadzacy.Imie.IndexOf(prowadzacy, StringComparison.CurrentCultureIgnoreCase) >= 0)
                             && wolneMiejsca <= (gz.Miejsca - gz.ZajeteMiejsca))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean SprawdzGrupeZProwadzacymWolne(string nazwaKursu, string potok, string kodGrupy, string kodKursu, string prowadzacy, long wolneMiejsca, GrupyZajeciowe gz)
        {
            if (SprawdzGrupe(nazwaKursu, potok, kodGrupy, kodGrupy, gz) &&
                            (gz.Prowadzacy.Nazwisko.IndexOf(prowadzacy, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                             gz.Prowadzacy.Imie.IndexOf(prowadzacy, StringComparison.CurrentCultureIgnoreCase) >= 0)
                             && wolneMiejsca <= (gz.Miejsca - gz.ZajeteMiejsca)
                             && gz.Wolna)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean SprawdzGrupeBezProwadzacego(string nazwaKursu, string potok, string kodGrupy, string kodKursu, string prowadzacy, long wolneMiejsca, GrupyZajeciowe gz)
        {
            if (SprawdzGrupe(nazwaKursu, potok, kodGrupy, kodGrupy, gz)
                            && prowadzacy.Equals("")
                            && wolneMiejsca <= (gz.Miejsca - gz.ZajeteMiejsca))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean SprawdzGrupeBezProwadzacegoWolne(string nazwaKursu, string potok, string kodGrupy, string kodKursu, string prowadzacy, long wolneMiejsca, GrupyZajeciowe gz)
        {
            if (SprawdzGrupe(nazwaKursu, potok, kodGrupy, kodGrupy, gz)
                            && prowadzacy.Equals("")
                            && wolneMiejsca <= (gz.Miejsca - gz.ZajeteMiejsca)
                            && gz.Wolna)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean SprawdzTypZajec(bool lab, bool cwiczenia, bool projekt, bool wyklad, GrupyZajeciowe gz)
        {
            if (lab && gz.TypZajec.Equals(labString) || (projekt && gz.TypZajec.Equals(projektString)) ||
                   (wyklad && gz.TypZajec.Equals(wykladString)) || (cwiczenia && gz.TypZajec.Equals(cwiczeniaString)))
                return true;
            else
                return false;
        }
        public Boolean SprawdzGrupe(string nazwaKursu, string potok, string kodGrupy, string kodKursu, GrupyZajeciowe gz)
        {
            if (gz.Kursy.NazwaKursu.IndexOf(nazwaKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 &&
                            gz.Potok.StartsWith(potok, StringComparison.OrdinalIgnoreCase) &&
                            gz.KodGrupy.IndexOf(kodGrupy, StringComparison.CurrentCultureIgnoreCase) >= 0 &&
                            gz.KodKursu.IndexOf(kodKursu, StringComparison.CurrentCultureIgnoreCase) >= 0)
                return true;
            else
                return false;
        }

        public void ZmienLiczbeMiejsc(string kodGrupy, long lMiejsc)
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                var gr = uw.GrupyZajeciowe.GetAll().ToList();
                var g = gr.Find(s => s.KodGrupy == kodGrupy);
                Debug.WriteLine(g.KodGrupy + " " + kodGrupy);
                Debug.WriteLine(g.KodGrupy + " " + g.KodKursu + " " + g.IdProwadzacego);
                g.ZajeteMiejsca = lMiejsc;
                var firstOrDefault = Items.FirstOrDefault(s => s.KodGrupy == kodGrupy);
                if (firstOrDefault != null)
                    firstOrDefault.ZajeteMiejsca = lMiejsc;
                //Items.Find(s => s.KodGrupy == kodGrupy).ZajeteMiejsca = lMiejsc;
                uw.SaveChanges();
            }
        }
    }
}