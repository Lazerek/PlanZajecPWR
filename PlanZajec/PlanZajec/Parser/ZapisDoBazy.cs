using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using System.Data.SqlClient;

namespace PlanZajec.Parser
{
    class ZapisDoBazy
    {
        public static void zapisz(GroupData gd)
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                var kr = NowyKurs(gd.KodKursu, gd.NazwaKursu,uw);
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
                String Nazwisko = tytulNazwiskoImie[count - 1];
                String Imie = tytulNazwiskoImie[count - 2];
                int j = 0;
                for (int i = pr.Length - 1; i > Nazwisko.Length + Imie.Length; i--)
                {
                    j++;
                }

                String Tytul = pr.Substring(0, j - 1);
                var pro = NowyProwadzacy(Imie, Nazwisko, Tytul,uw);

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
                String Tydzien = "Całkowity";
                if (Dzien.Count() > 3)
                    Tydzien = Dzien.Substring(3);
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
                        IdProwadzacego = pro.IdProwadzacego,
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
        static Kursy NowyKurs(String KodKursu, String NazwaKursu,UnitOfWork uw)
        {
           
            {
                int jest =-1;
                var table = uw.Kursy.GetAll();
                for(int i=0;i<table.Count();i++)
                {
                    if (table.ElementAt(i).KodKursu.Equals(KodKursu))
                        jest = i;
                }
                if(jest==-1)
                {
                    var kr = new Kursy() { KodKursu = KodKursu, NazwaKursu = NazwaKursu };
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
    }
}

