using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;

namespace PlanZajec.ViewModels
{
    public class ProwadzacyOpinieViewModel : INotifyPropertyChanged
    {
        public List<Prowadzacy> Items { get; set; }
        public List<Prowadzacy> ComboBoxItems { get; set; }

        public ProwadzacyOpinieViewModel()
        {
            ComboBoxItems = new List<Prowadzacy>();
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                ComboBoxItems = uw.Prowadzacy.GetAll().ToList();
            }
        }
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

        public void ZapiszOpinie(string pr, int index, string opinia, string ocena)
        {
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
                        if(double.TryParse(ocena, out ocenaDoZapisu))
                        {
                            if(ocenaDoZapisu >= 2.0f && ocenaDoZapisu <= 5.5f)
                                p.Ocena = double.Parse(ocena);
                        }             
                    }
                }
                uw.SaveChanges();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

