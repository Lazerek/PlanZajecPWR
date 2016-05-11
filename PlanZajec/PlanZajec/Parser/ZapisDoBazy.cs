using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using System.Data.SqlClient;
using Microsoft.Win32;
using System.IO;
using PlanZajec.ViewModels;

namespace PlanZajec.Parser
{
    class ZapisDoBazy
    {
        
        public static void zapisz(GroupData gd)
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                Blok blok=null;
                if (gd.KodBloku!="")
                {
                     blok = NowyBlok(gd.KodBloku, gd.NazwaBloku, uw);
                }

                var kr = NowyKurs(gd.KodKursu, gd.NazwaKursu,uw,blok);
                String pr = gd.Prowadzacy;
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
                String nazwisko = "";
                if (count - 1 >= 0)
                {
                    nazwisko = tytulNazwiskoImie[count - 1];
                }
                String imie = "";
                String tytul = "";
                if (count - 2 >= 0)
                {
                    imie = tytulNazwiskoImie[count - 2];
                    int j = 0;
                    for (int i = pr.Length - 1; i > nazwisko.Length + imie.Length; i--)
                    {
                        j++;
                    }
                    tytul = pr.Substring(0, j - 1);

                }
                Prowadzacy pro = null;
                if (nazwisko!="")
                {
                    pro = NowyProwadzacy(imie, nazwisko, tytul, uw);
                }



                //do rozbicia Miejsce na sale i budynek oraz Data na dzień oraz godzinę!!!
                    String Budynek = "";
                    String Sala = "";
                if (gd.Miejsce!=null)
                {
                    //BUD. C-6 SALA 130
                    var budynekSala = gd.Miejsce.Split(' ');
                    Budynek = budynekSala[2];
                    Sala = budynekSala[4];
                }

                var dzienGodzina = gd.Data.Split(' ');
                String Dzien = dzienGodzina[0];
                String Godzina = dzienGodzina[1];
                String GodzinaP = "";
                String GodzinaK = "";
                String Dzien1 = Dzien.Substring(0, 2);
                String Tydzien = "//";
                if (Dzien.Count() > 3)
                    Tydzien = Dzien.Substring(3,2);
                if (Godzina.Count()>8)
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
                long miejsca=0;
                long maxMiejsca=0;
                if (gd.LiczbaMiejsc != "")
                {
                    ileNaIle = gd.LiczbaMiejsc.Split('/');
                    miejsca = long.Parse(ileNaIle[0]);
                    maxMiejsca = long.Parse(ileNaIle[1]);
                }

                bool jest = false;

                var yolo = uw.GrupyZajeciowe.GetAll();
                for (int i = 0; i < yolo.Count(); i++)
                {
                    if (yolo.ElementAt(i).KodGrupy.Equals(gd.KodGrupy))
                        jest = true;
                }
              
                if (jest == false)
                {
                    uw.GrupyZajeciowe.Add(new GrupyZajeciowe()
                    {
                        KodGrupy = gd.KodGrupy,
                        TypZajec = gd.FormaZajec,
                        Dzień = Dzien1,
                        Tydzien = Tydzien,
                        Godzina = GodzinaP,
                        GodzinaKoniec = GodzinaK,
                        Sala = Sala,
                        Budynek = Budynek,
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
        /// To dodaje nowego kursu jeżeli nie ma go w bazie
        /// </summary>
        /// <param name="KodKursu"></param>
        /// <param name="NazwaKursu"></param>
        /// <returns></returns>
        static Kursy NowyKurs(String KodKursu, String NazwaKursu,UnitOfWork uw,Blok blok)
        {
           
            {
                string aaa = "";
                if (blok!=null)
                {
                    aaa = blok.NazwaBloku;
                }
                int jest =-1;
                var table = uw.Kursy.GetAll();
                for(int i=0;i<table.Count();i++)
                {
                    if (table.ElementAt(i).KodKursu.Equals(KodKursu))
                        jest = i;
                }
                if(jest==-1)
                {
                    var kr = new Kursy() { KodKursu = KodKursu, NazwaKursu = NazwaKursu, Blok=aaa,Blok1=blok };
                    uw.Kursy.Add(kr);
                    uw.SaveChanges();
                    return kr;

                }
                return table.ElementAt(jest);
            }

        }
        /// <summary>
        /// To dodaje nowego prowadzącego jeżeli nie ma go w bazie
        /// </summary>
        /// <param name="imie"></param>
        /// <param name="nazwisko"></param>
        /// <param name="tytul"></param>
        /// <returns></returns>
        static Prowadzacy NowyProwadzacy(String imie, String nazwisko, String tytul, UnitOfWork uw)
        {
          
            {
                int jest = -1;
                var table = uw.Prowadzacy.GetAll();
                for (int i = 0; i < table.Count(); i++)
                {
                    if (table.ElementAt(i).Nazwisko.Equals(nazwisko) && table.ElementAt(i).Imie.Equals(imie) && table.ElementAt(i).Tytul.Equals(tytul))
                        jest = i;
                }
                if (jest == -1)
                {
                    var pr = new Prowadzacy() { Nazwisko = nazwisko, Imie = imie, Tytul = tytul };
                    uw.Prowadzacy.Add(pr);
                    uw.SaveChanges();
                    return pr;
                }
                return table.ElementAt(jest);
            }
        }
        static Blok NowyBlok(String KodBloku, String NazwaBloku, UnitOfWork uw)
        {

            {
                int jest = -1;
                var table = uw.Bloki.GetAll();
                for (int i = 0; i < table.Count(); i++)
                {
                    if (table.ElementAt(i).KodBloku.Equals(KodBloku))
                        jest = i;
                }
                if (jest == -1)
                {
                    var bl = new Blok() { KodBloku = KodBloku, NazwaBloku = NazwaBloku };
                    uw.Bloki.Add(bl);
                    uw.SaveChanges();
                    return bl;

                }
                return table.ElementAt(jest);
            }

        }
        public static void export(SaveFileDialog sf,int IdP)
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                
                var plany = uw.Plany.GetAll();
                ICollection<GrupyZajeciowe> grupy = null;
                
                foreach(var p in plany)
                {
                    if (p.IdPlanu == IdP)
                        grupy = p.GrupyZajeciowe;
                }
                using (StreamWriter sw = new StreamWriter(sf.FileName))
                {
                    string zapis = "";
                    foreach(var grupa in grupy)
                    {
                        zapis = grupa.ToString();
                        if (grupa.Prowadzacy == null)
                            zapis += ",,,,,";

                        sw.WriteLine(zapis);
                        
                    }
                }
            }
        }
        public static void Importuj(OpenFileDialog sf)
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                using (StreamReader sw = new StreamReader(sf.FileName))
                {
                    Plany plan = new Plany() { NazwaPlanu = "Nowy" };
                    WyborPlanuViewModel.Instance.DodajPlan(plan);
                    uw.Plany.Add(plan);
                    uw.SaveChanges();
                    string line;
                    string[] dane;
                    while ((line = sw.ReadLine()) != null)
                    {
                        dane = line.Split(',');
                        var blok = NowyBlok(dane[14], dane[15], uw);
                       
                        var kr = NowyKurs(dane[11], dane[12], uw,blok);
                        if(dane[13]!="")
                            kr.ECTS =long.Parse(dane[13]);
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
                       
                        bool jest = false;
                        var yolo = uw.GrupyZajeciowe.GetAll();
                        GrupyZajeciowe gr=null;
                        for (int i = 0; i < yolo.Count(); i++)
                        {
                            if (yolo.ElementAt(i).KodGrupy.Equals(dane[0]))
                            {
                                jest = true;
                                gr = yolo.ElementAt(i);
                            }
                        }



                        if (jest == false)
                        {
                            if(IdP!=0)
                            gr =(new GrupyZajeciowe()
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
                                Miejsca =long.Parse( dane[8]),
                                ZajeteMiejsca = long.Parse( dane[9]),
                                IdProwadzacego = IdP,
                                KodKursu = kr.KodKursu,
                                Kursy = kr,
                                Prowadzacy = pro
                            });
                            else
                                gr = (new GrupyZajeciowe()
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
                                });


                        }
                        uw.GrupyZajeciowe.Add(gr);

                        uw.Plany.DodajGrupeZajeciowaDoPlanu(gr, plan.IdPlanu);
                        uw.SaveChanges();


                    }
                    
                }
            }
        }
    }
}

