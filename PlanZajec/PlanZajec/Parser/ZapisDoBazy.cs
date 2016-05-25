using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using PlanZajec.ViewModels;

namespace PlanZajec.Parser
{
    /// <summary>
    /// Klasa zapisująca dane grup zajęciowych do bazy
    /// </summary>
    internal class ZapisDoBazy
    {
        /// <summary>
        /// Metoda zapisująca dane do bazy
        /// </summary>
        /// <param name="gd">Objekt klasy groupdata zawierający dane o grupie zajęciowej</param>
        public static void zapisz(GroupData gd)
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                Blok blok = null;
                if (gd.KodBloku != "")
                {
                    blok = NowyBlok(gd.KodBloku, gd.NazwaBloku, uw);
                }

                var kr = NowyKurs(gd.KodKursu, gd.NazwaKursu, uw, blok);
                var pr = parseProwadzacy( gd.Prowadzacy);
                
                Prowadzacy pro = null;
                if (pr[0] != "")
                {
                    pro = NowyProwadzacy(pr[0], pr[1], pr[2], uw);
                }


                //do rozbicia Miejsce na sale i budynek oraz Data na dzień oraz godzinę!!!
                var budynek = "";
                var Sala = "";
                if (gd.Miejsce != null)
                {
                    //BUD. C-6 SALA 130
                    var budynekSala = gd.Miejsce.Split(' ');
                    budynek = budynekSala[2];
                    Sala = budynekSala[4];
                }

                var dzienGodzina = gd.Data.Split(' ');
                var Dzien = dzienGodzina[0];
                var Godzina = dzienGodzina[1];
                var GodzinaP = "";
                var GodzinaK = "";
                var Dzien1 = Dzien.Substring(0, 2);
                var Tydzien = "//";
                if (Dzien.Count() > 3)
                    Tydzien = Dzien.Substring(3, 2);
                if (Godzina.Count() > 8)
                {
                    GodzinaP = Godzina.Substring(0, 5);
                    GodzinaK = Godzina.Substring(6);
                }
                else
                {
                    GodzinaP = Godzina;
                    GodzinaK = Godzina;
                }

                var ileNaIle = new string[2];
                long miejsca = 0;
                long maxMiejsca = 0;
                if (gd.LiczbaMiejsc != "")
                {
                    ileNaIle = gd.LiczbaMiejsc.Split('/');
                    miejsca = long.Parse(ileNaIle[0]);
                    maxMiejsca = long.Parse(ileNaIle[1]);
                }

                var jest = false;

                var yolo = uw.GrupyZajeciowe.GetAll();
                for (var i = 0; i < yolo.Count(); i++)
                {
                    if (yolo.ElementAt(i).KodGrupy.Equals(gd.KodGrupy))
                        jest = true;
                }

                if (jest == false)
                {
                    uw.GrupyZajeciowe.Add(new GrupyZajeciowe
                    {
                        KodGrupy = gd.KodGrupy,
                        TypZajec = gd.FormaZajec,
                        Dzień = Dzien1,
                        Tydzien = Tydzien,
                        Godzina = GodzinaP,
                        GodzinaKoniec = GodzinaK,
                        Sala = Sala,
                        Budynek = budynek,
                        Potok = gd.Potok,
                        Miejsca = maxMiejsca,
                        ZajeteMiejsca = miejsca,
                        IdProwadzacego = pro?.IdProwadzacego,
                        KodKursu = kr.KodKursu,
                        Kursy = kr,
                        Prowadzacy = pro
                    });
                    uw.SaveChanges();
                }
            }
        }
        /// <summary>
        /// Metoda dzieląca String prowadzącego na dane atomowe
        /// </summary>
        /// <param name="pr">Dane prowadzącego</param>
        /// <returns>Tablica z tytułem, nazwiskiem i imieniem prowadzącego</returns>
        private static string[] parseProwadzacy(string pr)
        {
            string[] wynik=new string[3];
            var count = pr.Count(t => t.Equals(' '));
            var tytulNazwiskoImie = pr.Split(' ');
            count = tytulNazwiskoImie.Count();
            
            if (count - 1 >= 0)
            {
                wynik[0] = tytulNazwiskoImie[count - 1];
            }
            
            if (count - 2 >= 0)
            {
                wynik[1] = tytulNazwiskoImie[count - 2];
                var j = 0;
                for (var i = pr.Length - 1; i > wynik[0].Length + wynik[1].Length; i--)
                {
                    j++;
                }
                wynik[2] = pr.Substring(0, j - 1);
            }
            return wynik;
        }
        /// <summary>
        ///     To dodaje nowego kursu jeżeli nie ma go w bazie
        /// </summary>
        /// <param name="KodKursu">Kod Kursu</param>
        /// <param name="NazwaKursu">Nazwa kursu</param>
        /// <returns>Kurs</returns>
        private static Kursy NowyKurs(string KodKursu, string NazwaKursu, UnitOfWork uw, Blok blok)
        {

            {
                var aaa = "";
                if (blok != null)
                {
                    aaa = blok.NazwaBloku;
                }
                var jest = -1;
                var table = uw.Kursy.GetAll();
                for (var i = 0; i < table.Count(); i++)
                {
                    if (table.ElementAt(i).KodKursu.Equals(KodKursu))
                        jest = i;
                }
                if (jest == -1)
                {
                    var kr = new Kursy {KodKursu = KodKursu, NazwaKursu = NazwaKursu, Blok = aaa, Blok1 = blok};
                    uw.Kursy.Add(kr);
                    uw.SaveChanges();
                    return kr;
                }
                return table.ElementAt(jest);
            }
        }

        /// <summary>
        ///     To dodaje nowego prowadzącego jeżeli nie ma go w bazie
        /// </summary>
        /// <param name="imie">Imie prowadzącego</param>
        /// <param name="nazwisko">Nazwsiko prowadzącego</param>
        /// <param name="tytul">Tytuł prowadzącego</param>
        /// <returns>Prowadzącego</returns>
        private static Prowadzacy NowyProwadzacy(string imie, string nazwisko, string tytul, UnitOfWork uw)
        {

            {
                var jest = -1;
                var table = uw.Prowadzacy.GetAll();
                for (var i = 0; i < table.Count(); i++)
                {
                    if (table.ElementAt(i).Nazwisko.Equals(nazwisko) && table.ElementAt(i).Imie.Equals(imie) &&
                        table.ElementAt(i).Tytul.Equals(tytul))
                        jest = i;
                }
                if (jest == -1)
                {
                    var pr = new Prowadzacy {Nazwisko = nazwisko, Imie = imie, Tytul = tytul};
                    uw.Prowadzacy.Add(pr);
                    uw.SaveChanges();
                    return pr;
                }
                return table.ElementAt(jest);
            }
        }
        /// <summary>
        /// Dodanie nowego bloku do bazy jeśli go nie ma
        /// </summary>
        /// <param name="KodBloku">Kod bloku</param>
        /// <param name="NazwaBloku">Nazwa bloku</param>
        /// <param name="uw">Baza danych</param>
        /// <returns>Blok</returns>
        private static Blok NowyBlok(string KodBloku, string NazwaBloku, UnitOfWork uw)
        {

            {
                var jest = -1;
                var table = uw.Bloki.GetAll();
                for (var i = 0; i < table.Count(); i++)
                {
                    if (table.ElementAt(i).KodBloku.Equals(KodBloku))
                        jest = i;
                }
                if (jest == -1)
                {
                    var bl = new Blok {KodBloku = KodBloku, NazwaBloku = NazwaBloku};
                    uw.Bloki.Add(bl);
                    uw.SaveChanges();
                    return bl;
                }
                return table.ElementAt(jest);
            }
        }
        /// <summary>
        /// Metoda zapisująca plan do pliku
        /// </summary>
        /// <param name="sf">Dialog zapisu do pliku</param>
        /// <param name="IdP">Id Planu</param>
        public static void export(SaveFileDialog sf, long IdP)
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                var plany = uw.Plany.GetAll();
                ICollection<GrupyZajeciowe> grupy = null;
                string nazwa = "";
                string wolnedni = "";
                foreach (var p in plany)
                {
                    if (p.IdPlanu == IdP)
                    {
                        grupy = p.GrupyZajeciowe;
                        nazwa = p.NazwaPlanu;
                        wolnedni = p.WolneDni;

                    }
                }
                using (var sw = new StreamWriter(sf.FileName))
                {
                    sw.WriteLine(nazwa);
                    sw.WriteLine(wolnedni);
                    var zapis = "";
                    foreach (var grupa in grupy)
                    {
                        zapis = grupa.ToString();
                        if (grupa.Prowadzacy == null)
                            zapis += ",,,,,";

                        sw.WriteLine(zapis);
                    }
                }
            }
        }
        /// <summary>
        /// Metoda wczytująca plik z planem
        /// </summary>
        /// <param name="sf">Dialog otwierania pliku</param>
        public static void Importuj(OpenFileDialog sf)
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                using (var sw = new StreamReader(sf.FileName))
                {
                    var plan = new Plany {NazwaPlanu = sw.ReadLine()};
                    plan.WolneDni = sw.ReadLine();
                    WyborPlanuViewModel.Instance.DodajPlan(plan);
                    uw.Plany.Add(plan);
                    uw.SaveChanges();
                    string line;
                    string[] dane;
                    while ((line = sw.ReadLine()) != null)
                    {
                        dane = line.Split(',');
                        var blok = NowyBlok(dane[14], dane[15], uw);

                        var kr = NowyKurs(dane[11], dane[12], uw, blok);
                        if (dane[13] != "")
                            kr.ECTS = long.Parse(dane[13]);
                        Prowadzacy pro = null;
                        long IdP = 0;
                        if (dane[16] != "")
                        {
                            pro = NowyProwadzacy(dane[16], dane[17], dane[18], uw);
                            if (dane[19] != "")
                                pro.Ocena = long.Parse(dane[19]);
                            pro.Opis = dane[20];
                            IdP = pro.IdProwadzacego;
                        }

                        var jest = false;
                        var yolo = uw.GrupyZajeciowe.GetAll();
                        GrupyZajeciowe gr = null;
                        for (var i = 0; i < yolo.Count(); i++)
                        {
                            if (yolo.ElementAt(i).KodGrupy.Equals(dane[0]))
                            {
                                jest = true;
                                gr = yolo.ElementAt(i);
                            }
                        }


                        if (jest == false)
                        {
                            if (IdP != 0)
                                gr = new GrupyZajeciowe
                                {
                                    KodGrupy = dane[0],
                                    TypZajec = dane[1],
                                    Dzień = dane[2],
                                    Tydzien = dane[3],
                                    Godzina = dane[4],
                                    GodzinaKoniec = dane[5],
                                    Sala = dane[6],
                                    Budynek = dane[7],
                                    Potok = dane[10],
                                    Miejsca = long.Parse(dane[8]),
                                    ZajeteMiejsca = long.Parse(dane[9]),
                                    IdProwadzacego = IdP,
                                    KodKursu = kr.KodKursu,
                                    Kursy = kr,
                                    Prowadzacy = pro
                                };
                            else
                                gr = new GrupyZajeciowe
                                {
                                    KodGrupy = dane[0],
                                    TypZajec = dane[1],
                                    Dzień = dane[2],
                                    Tydzien = dane[3],
                                    Godzina = dane[4],
                                    GodzinaKoniec = dane[5],
                                    Sala = dane[6],
                                    Budynek = dane[7],
                                    Potok = dane[10],
                                    Miejsca = long.Parse(dane[8]),
                                    ZajeteMiejsca = long.Parse(dane[9]),
                                    //  IdProwadzacego = IdP,
                                    KodKursu = kr.KodKursu,
                                    Kursy = kr,
                                    Prowadzacy = pro
                                };
                            uw.GrupyZajeciowe.Add(gr);
                            uw.SaveChanges();
                        }
                        
                        uw.Plany.DodajGrupeZajeciowaDoPlanu(gr, plan.IdPlanu);
                        uw.SaveChanges();
                    }
                }
            }
        }
    }
}