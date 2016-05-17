using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PlanZajec.DataModel;
using PlanZajec.DataAccessLayer;

namespace PlanZajec.ViewModels
{

    public class PrzegladanieGrupViewModel : ViewModel, INotifyPropertyChanged
    {
        public ObservableCollection<GrupyZajeciowe> Items { get; set; }
        public GrupyZajeciowe SelectedItem { get; set; }

        public List<GrupyZajeciowe> ItemsNoChange { get; set; }
        public static PrzegladanieGrupViewModel przegladanieGrupViewModel;

        public List<GrupyZajeciowe> ItemsChanged { get; set; }
        public static PrzegladanieGrupViewModel temp;
        public PrzegladanieGrupViewModel()
        {
            przegladanieGrupViewModel = this;
            
            temp = this;
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                Items = new ObservableCollection<GrupyZajeciowe>(uw.GrupyZajeciowe.GetGrupyZajecioweWithRelations().ToList());
                ItemsNoChange = uw.GrupyZajeciowe.GetGrupyZajecioweWithRelations().ToList();
            }
        }

        public void reloadData()
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                Items = new ObservableCollection<GrupyZajeciowe>(uw.GrupyZajeciowe.GetGrupyZajecioweWithRelations().ToList());
                ItemsNoChange = uw.GrupyZajeciowe.GetGrupyZajecioweWithRelations().ToList();
            }
        }
        public void czyscFiltrownie()
        {
            Items.Clear();
            Items = new ObservableCollection<GrupyZajeciowe>();
            foreach(GrupyZajeciowe gz in ItemsNoChange)
            {
                Items.Add(gz);
            }
            NotifyPropertyChange("Items");
        }
        public void Filtruj(String nazwaKursu, String potok, String kodGrupy, String kodKursu, String prowadzacy)
        {
            ItemsChanged = new List<GrupyZajeciowe>();
            foreach(GrupyZajeciowe gz in ItemsNoChange)
            {
                if (gz.Kursy.NazwaKursu.IndexOf(nazwaKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && gz.Potok.StartsWith(potok, StringComparison.OrdinalIgnoreCase) && gz.KodGrupy.IndexOf(kodGrupy, StringComparison.CurrentCultureIgnoreCase) >= 0 &&gz.KodKursu.IndexOf(kodKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && (gz.Prowadzacy.Nazwisko.IndexOf(prowadzacy, StringComparison.CurrentCultureIgnoreCase) >= 0 || gz.Prowadzacy.Imie.IndexOf(prowadzacy, StringComparison.CurrentCultureIgnoreCase) >= 0))
                    ItemsChanged.Add(gz);
            }
            Items.Clear();
            Items = new ObservableCollection<GrupyZajeciowe>();
            foreach (GrupyZajeciowe gz in ItemsChanged)
                Items.Add(gz);
            NotifyPropertyChange("Items");
        }
        public void Filtruj2(String nazwaKursu, String potok, String kodGrupy, String kodKursu, String prowadzacy, String wolneMiejsca, Boolean lab, Boolean cwiczenia, Boolean projekt, Boolean wszystko, Boolean wyklad, Boolean wolne)
        {
            ItemsChanged = new List<GrupyZajeciowe>();
            String labString = "Zajęcia laboratoryjne";
            String projektString = "Projekt";
            String cwiczeniaString = "Ćwiczenia";
            String wykladString = "Wykład";
            //long result=-1;
            long result;
            Int64.TryParse(wolneMiejsca, out result);
            // System.Diagnostics.Debug.WriteLine(result);
            if(wolne)
                {
                if (wszystko)
                {
                    foreach (GrupyZajeciowe gz in ItemsNoChange)
                    {
                        if (result == 0)
                        {
                            if (gz.Prowadzacy != null)
                                if (gz.Kursy.NazwaKursu.IndexOf(nazwaKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && gz.Potok.StartsWith(potok, StringComparison.OrdinalIgnoreCase) && gz.KodGrupy.IndexOf(kodGrupy, StringComparison.CurrentCultureIgnoreCase) >= 0 &&gz.KodKursu.IndexOf(kodKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && (gz.Prowadzacy.Nazwisko.IndexOf(prowadzacy, StringComparison.CurrentCultureIgnoreCase) >= 0 || gz.Prowadzacy.Imie.IndexOf(prowadzacy, StringComparison.CurrentCultureIgnoreCase) >= 0))
                                    ItemsChanged.Add(gz);
                                else
                                if (gz.Kursy.NazwaKursu.IndexOf(nazwaKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && gz.Potok.StartsWith(potok, StringComparison.OrdinalIgnoreCase) && gz.KodGrupy.IndexOf(kodGrupy, StringComparison.CurrentCultureIgnoreCase) >= 0 &&gz.KodKursu.IndexOf(kodKursu, StringComparison.CurrentCultureIgnoreCase) >= 0)
                                    ItemsChanged.Add(gz);
                        }
                        /*
                        gz.KodGrupy.IndexOf(KodGrupy, StringComparison.CurrentCultureIgnoreCase) >= 0
                        gz.KodGrupy.IndexOf(KodGrupy, StringComparison.CurrentCultureIgnoreCase) >= 0
                       gz.KodKursu.IndexOf(KodKursu, StringComparison.CurrentCultureIgnoreCase) >= 0
                        gz.KodKursu.IndexOf(KodKursu, StringComparison.CurrentCultureIgnoreCase) >= 0
                        */
                        else
                        {
                            if (gz.Prowadzacy != null)
                                if (gz.Kursy.NazwaKursu.IndexOf(nazwaKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && gz.Potok.StartsWith(potok, StringComparison.OrdinalIgnoreCase) && gz.KodGrupy.IndexOf(kodGrupy, StringComparison.CurrentCultureIgnoreCase) >= 0 &&gz.KodKursu.IndexOf(kodKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && (gz.Prowadzacy.Nazwisko.IndexOf(prowadzacy, StringComparison.CurrentCultureIgnoreCase) >= 0 || gz.Prowadzacy.Imie.IndexOf(prowadzacy, StringComparison.CurrentCultureIgnoreCase) >= 0) && result <= (gz.Miejsca - gz.ZajeteMiejsca))
                                    ItemsChanged.Add(gz);
                                else
                                if (gz.Kursy.NazwaKursu.IndexOf(nazwaKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && gz.Potok.StartsWith(potok, StringComparison.OrdinalIgnoreCase) && gz.KodGrupy.IndexOf(kodGrupy, StringComparison.CurrentCultureIgnoreCase) >= 0 &&gz.KodKursu.IndexOf(kodKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && result <= (gz.Miejsca - gz.ZajeteMiejsca))
                                    ItemsChanged.Add(gz);
                        }
                    }
                }
                else
                {
                    foreach (GrupyZajeciowe gz in ItemsNoChange)
                    {
                        if (lab && (gz.TypZajec.Equals(labString)) || (projekt && gz.TypZajec.Equals(projektString)) || (wyklad && gz.TypZajec.Equals(wykladString)) || (cwiczenia && gz.TypZajec.Equals(cwiczeniaString)))
                        {
                            if (result == 0)
                            {
                                if (gz.Prowadzacy != null)
                                    if (gz.Kursy.NazwaKursu.IndexOf(nazwaKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && gz.Potok.StartsWith(potok, StringComparison.OrdinalIgnoreCase) && gz.KodGrupy.IndexOf(kodGrupy, StringComparison.CurrentCultureIgnoreCase) >= 0 &&gz.KodKursu.IndexOf(kodKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && (gz.Prowadzacy.Nazwisko.IndexOf(prowadzacy, StringComparison.CurrentCultureIgnoreCase) >= 0 || gz.Prowadzacy.Imie.IndexOf(prowadzacy, StringComparison.CurrentCultureIgnoreCase) >= 0))
                                        ItemsChanged.Add(gz);
                                    else
                                    if (gz.Kursy.NazwaKursu.IndexOf(nazwaKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && gz.Potok.StartsWith(potok, StringComparison.OrdinalIgnoreCase) && gz.KodGrupy.IndexOf(kodGrupy, StringComparison.CurrentCultureIgnoreCase) >= 0 &&gz.KodKursu.IndexOf(kodKursu, StringComparison.CurrentCultureIgnoreCase) >= 0)
                                        ItemsChanged.Add(gz);
                            }
                            else
                            {
                                if (gz.Prowadzacy != null)
                                    if (gz.Kursy.NazwaKursu.IndexOf(nazwaKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && gz.Potok.StartsWith(potok, StringComparison.OrdinalIgnoreCase) && gz.KodGrupy.IndexOf(kodGrupy, StringComparison.CurrentCultureIgnoreCase) >= 0 &&gz.KodKursu.IndexOf(kodKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && (gz.Prowadzacy.Nazwisko.IndexOf(prowadzacy, StringComparison.CurrentCultureIgnoreCase) >= 0 || gz.Prowadzacy.Imie.IndexOf(prowadzacy, StringComparison.CurrentCultureIgnoreCase) >= 0) && result <= (gz.Miejsca - gz.ZajeteMiejsca))
                                        ItemsChanged.Add(gz);
                                    else
                                    if (gz.Kursy.NazwaKursu.IndexOf(nazwaKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && gz.Potok.StartsWith(potok, StringComparison.OrdinalIgnoreCase) && gz.KodGrupy.IndexOf(kodGrupy, StringComparison.CurrentCultureIgnoreCase) >= 0 &&gz.KodKursu.IndexOf(kodKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && result <= (gz.Miejsca - gz.ZajeteMiejsca))
                                        ItemsChanged.Add(gz);
                            }
                        }
                    }
                }
            }
            else
            {
                if (wszystko)
                {
                    foreach (GrupyZajeciowe gz in ItemsNoChange)
                    {
                        if (result == 0)
                        {
                            if(gz.Prowadzacy!=null)
                                if (gz.Kursy.NazwaKursu.IndexOf(nazwaKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && gz.Potok.StartsWith(potok, StringComparison.OrdinalIgnoreCase) && gz.KodGrupy.IndexOf(kodGrupy, StringComparison.CurrentCultureIgnoreCase) >= 0 &&gz.KodKursu.IndexOf(kodKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && (gz.Prowadzacy.Nazwisko.IndexOf(prowadzacy, StringComparison.CurrentCultureIgnoreCase) >= 0 || gz.Prowadzacy.Imie.IndexOf(prowadzacy, StringComparison.CurrentCultureIgnoreCase) >= 0))
                                    ItemsChanged.Add(gz);
                            else
                                if(gz.Kursy.NazwaKursu.IndexOf(nazwaKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && gz.Potok.StartsWith(potok, StringComparison.OrdinalIgnoreCase) && gz.KodGrupy.IndexOf(kodGrupy, StringComparison.CurrentCultureIgnoreCase) >= 0 &&gz.KodKursu.IndexOf(kodKursu, StringComparison.CurrentCultureIgnoreCase) >= 0)
                                    ItemsChanged.Add(gz);
                        }
                        else
                        {
                            if(gz.Prowadzacy!=null)
                                if (gz.Kursy.NazwaKursu.IndexOf(nazwaKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && gz.Potok.StartsWith(potok, StringComparison.OrdinalIgnoreCase) && gz.KodGrupy.IndexOf(kodGrupy, StringComparison.CurrentCultureIgnoreCase) >= 0 &&gz.KodKursu.IndexOf(kodKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && (gz.Prowadzacy.Nazwisko.IndexOf(prowadzacy, StringComparison.CurrentCultureIgnoreCase) >= 0 || gz.Prowadzacy.Imie.IndexOf(prowadzacy, StringComparison.CurrentCultureIgnoreCase) >= 0) && result <= (gz.Miejsca - gz.ZajeteMiejsca))
                                    ItemsChanged.Add(gz);
                            else
                                if (gz.Kursy.NazwaKursu.IndexOf(nazwaKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && gz.Potok.StartsWith(potok, StringComparison.OrdinalIgnoreCase) && gz.KodGrupy.IndexOf(kodGrupy, StringComparison.CurrentCultureIgnoreCase) >= 0 &&gz.KodKursu.IndexOf(kodKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 &&  result <= (gz.Miejsca - gz.ZajeteMiejsca))
                                    ItemsChanged.Add(gz);
                        }
                    }
                }
                else
                {
                    foreach (GrupyZajeciowe gz in ItemsNoChange)
                    {
                        if (lab && (gz.TypZajec.Equals(labString)) || (projekt && gz.TypZajec.Equals(projektString)) || (wyklad && gz.TypZajec.Equals(wykladString)) || (cwiczenia && gz.TypZajec.Equals(cwiczeniaString)))
                        {
                            if (result == 0)
                            {
                                if (gz.Prowadzacy != null)
                                    if (gz.Kursy.NazwaKursu.IndexOf(nazwaKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && gz.Potok.StartsWith(potok, StringComparison.OrdinalIgnoreCase) && gz.KodGrupy.IndexOf(kodGrupy, StringComparison.CurrentCultureIgnoreCase) >= 0 &&gz.KodKursu.IndexOf(kodKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && (gz.Prowadzacy.Nazwisko.IndexOf(prowadzacy, StringComparison.CurrentCultureIgnoreCase) >= 0 || gz.Prowadzacy.Imie.IndexOf(prowadzacy, StringComparison.CurrentCultureIgnoreCase) >= 0))
                                        ItemsChanged.Add(gz);
                                    else
                                    if (gz.Kursy.NazwaKursu.IndexOf(nazwaKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && gz.Potok.StartsWith(potok, StringComparison.OrdinalIgnoreCase) && gz.KodGrupy.IndexOf(kodGrupy, StringComparison.CurrentCultureIgnoreCase) >= 0 &&gz.KodKursu.IndexOf(kodKursu, StringComparison.CurrentCultureIgnoreCase) >= 0)
                                        ItemsChanged.Add(gz);
                            }
                            else
                            {
                                if (gz.Prowadzacy != null)
                                    if (gz.Kursy.NazwaKursu.IndexOf(nazwaKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && gz.Potok.StartsWith(potok, StringComparison.OrdinalIgnoreCase) && gz.KodGrupy.IndexOf(kodGrupy, StringComparison.CurrentCultureIgnoreCase) >= 0 &&gz.KodKursu.IndexOf(kodKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && (gz.Prowadzacy.Nazwisko.IndexOf(prowadzacy, StringComparison.CurrentCultureIgnoreCase) >= 0 || gz.Prowadzacy.Imie.IndexOf(prowadzacy, StringComparison.CurrentCultureIgnoreCase) >= 0) && result <= (gz.Miejsca - gz.ZajeteMiejsca))
                                        ItemsChanged.Add(gz);
                                    else
                                    if (gz.Kursy.NazwaKursu.IndexOf(nazwaKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && gz.Potok.StartsWith(potok, StringComparison.OrdinalIgnoreCase) && gz.KodGrupy.IndexOf(kodGrupy, StringComparison.CurrentCultureIgnoreCase) >= 0 &&gz.KodKursu.IndexOf(kodKursu, StringComparison.CurrentCultureIgnoreCase) >= 0 && result <= (gz.Miejsca - gz.ZajeteMiejsca))
                                        ItemsChanged.Add(gz);
                            }
                        }
                    }
                }
            }
           
            Items.Clear();
            Items = new ObservableCollection<GrupyZajeciowe>();
            foreach (GrupyZajeciowe gz in ItemsChanged)
                Items.Add(gz);
            ItemsChanged.Clear();
            NotifyPropertyChange("Items");
        }
        /*(public void Filtruj(String nazwaKursu, String potok, String kodGrupy, String kodKursu, String prowadzacy)
        {
            ItemsChanged = new List<GrupyZajeciowe>();
            List<Kursy> kursy = new List<Kursy>();
            List<Prowadzacy> prowadzacyList = new List<Prowadzacy>();
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                Items = uw.GrupyZajeciowe.GetAll().ToList();
                kursy = uw.Kursy.GetAll().ToList();
                prowadzacyList = uw.Prowadzacy.GetAll().ToList();
            }
            if(nazwaKursu.Equals("") && potok.Equals("") && kodGrupy.Equals("") && kodKursu.Equals(""))
            {
                foreach (GrupyZajeciowe gz in ItemsNoChange)
                    ItemsChanged.Add(gz);
            }
            foreach(GrupyZajeciowe gz in ItemsNoChange)
            {
                foreach(Kursy kr in kursy)
                {
                    if (kr.NazwaKursu.IndexOf(nazwaKursu, StringComparison.CurrentCultureIgnoreCase) >=0 && gz.KodKursu==kr.KodKursu && gz.Potok.StartsWith(potok, StringComparison.OrdinalIgnoreCase) && gz.KodGrupy.IndexOf(KodGrupy, StringComparison.CurrentCultureIgnoreCase) >= 0 &&gz.KodKursu.IndexOf(KodKursu, StringComparison.CurrentCultureIgnoreCase) >= 0)
                        ItemsChanged.Add(gz);
                }
            }
            Items.Clear();
            Items = new List<GrupyZajeciowe>();
            if (!prowadzacy.Equals(""))
            {
                foreach (GrupyZajeciowe gz in ItemsChanged)
                {
                    foreach (Prowadzacy pr in prowadzacyList)
                    {
                        if (pr.IdProwadzacego == gz.IdProwadzacego && pr.Nazwisko.IndexOf(prowadzacy, StringComparison.CurrentCultureIgnoreCase) >= 0)
                            Items.Add(gz);
                    }
                }
            }
            else
            {
                foreach (GrupyZajeciowe gz in ItemsChanged)
                    Items.Add(gz);
            }
            ItemsChanged.Clear();
            NotifyPropertyChange("Items");
        }
        */
        public void FiltrCheckBox(Boolean lab, Boolean cwiczenia, Boolean projekt, Boolean wszystko, Boolean wyklad)
        {
            ItemsChanged = new List<GrupyZajeciowe>();
            String labString = "Zajęcia laboratoryjne";
            String projektString = "Projekt";
            String cwiczeniaString = "Ćwiczenia";
            String wykladString = "Wykład";
            if(wszystko)
            {
                foreach (GrupyZajeciowe gz in ItemsNoChange)
                    ItemsChanged.Add(gz);
            }
            else
            {
                foreach (GrupyZajeciowe gz in ItemsNoChange)
                {
                    if (lab && (gz.TypZajec.Equals(labString)) || (projekt && gz.TypZajec.Equals(projektString)) || (wyklad && gz.TypZajec.Equals(wykladString)) || (cwiczenia && gz.TypZajec.Equals(cwiczeniaString)))
                    {
                        ItemsChanged.Add(gz);
                    }
                }
            }
            Items.Clear();
            Items = new ObservableCollection<GrupyZajeciowe>();
            foreach (GrupyZajeciowe gz in ItemsChanged)
                Items.Add(gz);
            ItemsChanged.Clear();
            NotifyPropertyChange("Items");
        }

        public void ZmienLiczbeMiejsc(string kodGrupy, long lMiejsc)
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                var gr = uw.GrupyZajeciowe.GetAll().ToList();
                GrupyZajeciowe g = gr.Find(s => s.KodGrupy == kodGrupy);
                System.Diagnostics.Debug.WriteLine(g.KodGrupy + " " + kodGrupy);
                System.Diagnostics.Debug.WriteLine(g.KodGrupy + " " + g.KodKursu + " " + g.IdProwadzacego);
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
