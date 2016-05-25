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
    /// <summary>
    /// Klasa ViewModel odpowiedzialna za przeglądanie dostępnych grup zajęciowych
    /// </summary>
    public class PrzegladanieGrupViewModel : ViewModel, INotifyPropertyChanged
    {
        public static PrzegladanieGrupViewModel przegladanieGrupViewModel;
        public static PrzegladanieGrupViewModel temp;
        private readonly string cwiczeniaString = "Ćwiczenia";
        private readonly string labString = "Zajęcia laboratoryjne";
        private readonly string projektString = "Projekt";
        private readonly string wykladString = "Wykład";

        /// <summary>
        /// Konstruktor domyślny, który wczytuje grupy zajęciowe z bazy danych
        /// </summary>
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
        /// <summary>
        /// Metoda do przeładowania danych
        /// </summary>
        public void reloadData()
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                Items =
                    new ObservableCollection<GrupyZajeciowe>(uw.GrupyZajeciowe.GetGrupyZajecioweWithRelations().ToList());
                ItemsNoChange = uw.GrupyZajeciowe.GetGrupyZajecioweWithRelations().ToList();
            }
        }
        /// <summary>
        /// Metoda czyszcząca filtrowanie
        /// </summary>
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

        /// <summary>
        /// Metoda odpowiedzialna za filtrowanie
        /// </summary>
        /// <param name="nazwaKursu">Nazwa Kursu</param>
        /// <param name="potok">Potok</param>
        /// <param name="kodGrupy">Kod Grupy</param>
        /// <param name="kodKursu">Kod Kursu</param>
        /// <param name="prowadzacy">Prowadzacy</param>
        /// <param name="wolneMiejsca">Wolne Miejsca w grupach zajęciowych</param>
        /// <param name="lab">Zmienna informująca o szukaniu laboratoriów</param>
        /// <param name="cwiczenia">Zmienna informująca o szukaniu ćwiczeń</param>
        /// <param name="projekt">Zmienna informująca o szukaniu projektu</param>
        /// <param name="wszystko">Zmienna informująca o szukaniu wszystkich typów zajęć</param>
        /// <param name="wyklad">Zmienna informująca o szukaniu jedynie wykładów</param>
        /// <param name="wolne">Zmienna mówiąca o szukaniu tylko wolnych grup</param>
        public void Filtruj2(string nazwaKursu, string potok, string kodGrupy, string kodKursu, string prowadzacy,
            string wolneMiejsca, bool lab, bool cwiczenia, bool projekt, bool wszystko, bool wyklad, bool wolne)
        {
            long result;
            long.TryParse(wolneMiejsca, out result);
            if (result == 0)
                result = -1000;
            if (wolne && wszystko)
                FiltrujWolneWszystko(nazwaKursu, potok, kodGrupy, kodKursu, prowadzacy, result);
            else if (wolne && !wszystko)
                FiltrujWolneNieWszystko(nazwaKursu, potok, kodGrupy, kodKursu, prowadzacy, lab, cwiczenia, projekt, wszystko, wyklad, result);
            else if (!wolne && wszystko)
                FiltrujZajeteWszystko(nazwaKursu, potok, kodGrupy, kodKursu, prowadzacy, result);
            else if (!wolne && !wszystko)
                FiltrujZajeteNieWszystko(nazwaKursu, potok, kodGrupy, kodKursu, prowadzacy, lab, cwiczenia, projekt, wszystko, wyklad, result);

            Items.Clear();
            Items = new ObservableCollection<GrupyZajeciowe>();
            foreach (var gz in ItemsChanged)
                Items.Add(gz);
            ItemsChanged.Clear();
            NotifyPropertyChange("Items");
        }
        /// <summary>
        /// Funkcja pomocnicza filtrująca wszystkie typy zajeć i tylko wolne grupy
        /// </summary>
        /// <param name="nazwaKursu">Nazwa Kursu</param>
        /// <param name="potok">Potok</param>
        /// <param name="kodGrupy">Kod Grupy</param>
        /// <param name="kodKursu">Kod Kursu</param>
        /// <param name="prowadzacy">Prowadzacy</param>
        /// <param name="result">Liczba szukanych wolnych miejsc w grupie</param>
        public void FiltrujWolneWszystko(string nazwaKursu, string potok, string kodGrupy, string kodKursu,
            string prowadzacy, long result)
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
        /// <summary>
        /// Metoda pomocnicza filtrująca tylko wolne grupy z różnymi typami zajęć
        /// </summary>
        /// <param name="nazwaKursu">Nazwa Kursu</param>
        /// <param name="potok">Potok</param>
        /// <param name="kodGrupy">Kod Grupy</param>
        /// <param name="kodKursu">Kod Kursu</param>
        /// <param name="prowadzacy">Prowadzacy</param>
        /// <param name="lab">Zmienna informująca o szukaniu laboratoriów</param>
        /// <param name="cwiczenia">Zmienna informująca o szukaniu ćwiczeń</param>
        /// <param name="projekt">Zmienna informująca o szukaniu projektu</param>
        /// <param name="wszystko">Zmienna informująca o szukaniu wszystkich typów zajęć</param>
        /// <param name="wyklad">Zmienna informująca o szukaniu jedynie wykładów</param>
        /// <param name="wolne">Zmienna mówiąca o szukaniu tylko wolnych grup</param>
        /// <param name="result">Liczba szukanych wolnych miejsc w grupie</param>
        public void FiltrujWolneNieWszystko(string nazwaKursu, string potok, string kodGrupy, string kodKursu,
            string prowadzacy, bool lab, bool cwiczenia, bool projekt, bool wszystko, bool wyklad,
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
        /// <summary>
        /// Metoda pomocnicza filtrująca wszystkie grupy
        /// </summary>
        /// <param name="nazwaKursu">Nazwa Kursu</param>
        /// <param name="potok">Potok</param>
        /// <param name="kodGrupy">Kod Grupy</param>
        /// <param name="kodKursu">Kod Kursu</param>
        /// <param name="prowadzacy">Prowadzacy</param>
		/// <param name="result">Liczba szukanych wolnych miejsc w grupie</param>
        public void FiltrujZajeteWszystko(string nazwaKursu, string potok, string kodGrupy, string kodKursu,
            string prowadzacy, long result)
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
        /// <summary>
        /// Pomocnicza metoda filtrująca wybrane typy zajęć
        /// </summary>
        /// <param name="nazwaKursu">Nazwa Kursu</param>
        /// <param name="potok">Potok</param>
        /// <param name="kodGrupy">Kod Grupy</param>
        /// <param name="kodKursu">Kod Kursu</param>
        /// <param name="prowadzacy">Prowadzacy</param>
        /// <param name="lab">Zmienna informująca o szukaniu laboratoriów</param>
        /// <param name="cwiczenia">Zmienna informująca o szukaniu ćwiczeń</param>
        /// <param name="projekt">Zmienna informująca o szukaniu projektu</param>
        /// <param name="wszystko">Zmienna informująca o szukaniu wszystkich typów zajęć</param>
        /// <param name="wyklad">Zmienna informująca o szukaniu jedynie wykładów</param>
        /// <param name="wolne">Zmienna mówiąca o szukaniu tylko wolnych grup</param>
        /// <param name="result">Liczba szukanych wolnych miejsc w grupie</param>
        public void FiltrujZajeteNieWszystko(string nazwaKursu, string potok, string kodGrupy, string kodKursu,
            string prowadzacy, bool lab, bool cwiczenia, bool projekt, bool wszystko, bool wyklad,
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
        /// <summary>
        /// Metoda pomocznicza do sprawdzania czy grupa zajęciowa jest odpowiednia, z posiadanym prowadzącym
        /// </summary>
        /// <param name="nazwaKursu">Nazwa Kursu</param>
        /// <param name="potok">Potok</param>
        /// <param name="kodGrupy">Kod Grupy</param>
        /// <param name="kodKursu">Kod Kursu</param>
        /// <param name="prowadzacy">Prowadzący</param>
        /// <param name="wolneMiejsca">Liczba szukanych wolnych miejsc</param>
        /// <param name="gz">Sprawdzana grupa zajęciowa</param>
        /// <returns>Pomyślność filtrowania grupy</returns>
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
        /// <summary>
        /// Metoda pomocznicza do sprawdzania czy grupa zajęciowa jest odpowiednia, z posiadanym prowadzącym, szukanie tylko wolnych grup
        /// </summary>
        /// <param name="nazwaKursu">Nazwa Kursu</param>
        /// <param name="potok">Potok</param>
        /// <param name="kodGrupy">Kod Grupy</param>
        /// <param name="kodKursu">Kod Kursu</param>
        /// <param name="prowadzacy">Prowadzący</param>
        /// <param name="wolneMiejsca">Liczba szukanych wolnych miejsc</param>
        /// <param name="gz">Sprawdzana grupa zajęciowa</param>
        /// <returns>Pomyślność filtrowania grupy</returns>
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
        /// <summary>
        /// Metoda pomocznicza do sprawdzania czy grupa zajęciowa jest odpowiednia, bez wpisanego prowadzącego
        /// </summary>
        /// <param name="nazwaKursu">Nazwa Kursu</param>
        /// <param name="potok">Potok</param>
        /// <param name="kodGrupy">Kod Grupy</param>
        /// <param name="kodKursu">Kod Kursu</param>
        /// <param name="prowadzacy">Prowadzący</param>
        /// <param name="wolneMiejsca">Liczba szukanych wolnych miejsc</param>
        /// <param name="gz">Sprawdzana grupa zajęciowa</param>
        /// <returns>Pomyślność filtrowania grupy</returns>
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
        /// <summary>
        /// Metoda pomocznicza do sprawdzania czy grupa zajęciowa jest odpowiednia, bez posiadania prowadzącego, szukanie tylko grup wolnych
        /// </summary>
        /// <param name="nazwaKursu">Nazwa Kursu</param>
        /// <param name="potok">Potok</param>
        /// <param name="kodGrupy">Kod Grupy</param>
        /// <param name="kodKursu">Kod Kursu</param>
        /// <param name="prowadzacy">Prowadzący</param>
        /// <param name="wolneMiejsca">Liczba szukanych wolnych miejsc</param>
        /// <param name="gz">Sprawdzana grupa zajęciowa</param>
        /// <returns>Pomyślność filtrowania grupy</returns>
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
        /// <summary>
        /// Metoda sprawdzająca czy grupa mieści się w określonych typach zajęć
        /// </summary>
        /// <param name="lab"></param>
        /// <param name="cwiczenia"></param>
        /// <param name="projekt"></param>
        /// <param name="wyklad"></param>
        /// <param name="gz">grpa zajęciowa</param>
        /// <returns>Zgodność grupy z typem zajęć</returns>
        public Boolean SprawdzTypZajec(bool lab, bool cwiczenia, bool projekt, bool wyklad, GrupyZajeciowe gz)
        {
            if (lab && gz.TypZajec.Equals(labString) || (projekt && gz.TypZajec.Equals(projektString)) ||
                   (wyklad && gz.TypZajec.Equals(wykladString)) || (cwiczenia && gz.TypZajec.Equals(cwiczeniaString)))
                return true;
            else
                return false;
        }
        /// <summary>
        /// Sprawdzanie czy grupa spełnia wymagania filtrowania
        /// </summary>
        /// <param name="nazwaKursu"></param>
        /// <param name="potok"></param>
        /// <param name="kodGrupy"></param>
        /// <param name="kodKursu"></param>
        /// <param name="gz"></param>
        /// <returns>Zgodność grupy z danymi do filtrowania</returns>
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
        /// <summary>
        /// Metoda pozwalająca zmienić liczbę miejsc w grupie
        /// </summary>
        /// <param name="kodGrupy">Kod Grupy zajęciowej</param>
        /// <param name="lMiejsc">Nowa liczba miejsc w grupie</param>
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