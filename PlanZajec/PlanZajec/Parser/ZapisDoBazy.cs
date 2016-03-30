﻿using System;
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
                var kr = NowyKurs(gd.KodKursu, gd.NazwaKursu);
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
                var pro = NowyProwadzacy(Imie, Nazwisko, Tytul);
                //do rozbicia Miejsce na sale i budynek oraz Data na dzień oraz godzinę!!!
                uw.GrupyZajeciowe.Add(new GrupyZajeciowe() { KodGrupy = gd.KodGrupy, TypZajec = gd.FormaZajec, Dzień=gd.Data, Godzina=gd.Data, Sala=gd.Miejsce, Budynek=gd.Miejsce, Prowadzacy=pro, Kursy=kr });
                uw.SaveChanges();
              
                                               
            }

        }
       static Kursy NowyKurs(String KodKursu, String NazwaKursu)
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
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
                    var kr = new Kursy() {KodKursu = KodKursu, NazwaKursu = NazwaKursu};
                    uw.Kursy.Add(kr);
                    uw.SaveChanges();
                    return kr;

                }
                return table.ElementAt(jest);
            }

        }
        static Prowadzacy NowyProwadzacy(String imie, String nazwisko, String tytul)
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
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
                    var pr = new Prowadzacy() {Nazwisko = nazwisko, Imie = imie, Tytul = tytul};
                    uw.Prowadzacy.Add(pr);
                    uw.SaveChanges();
                    return pr;
                }
                return table.ElementAt(jest);
            }
        }
    }
}

