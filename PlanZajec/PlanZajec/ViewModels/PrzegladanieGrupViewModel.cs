using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PlanZajec.DataModel;
using PlanZajec.DataAccessLayer;

namespace PlanZajec.ViewModels
{

    public class PrzegladanieGrupViewModel : ViewModel
    {
        public List<GrupyZajeciowe> Items { get; set; }
        public static PrzegladanieGrupViewModel przegladanieGrupViewModel;

        public List<GrupyZajeciowe> ItemsChanged { get; set; }
        public static PrzegladanieGrupViewModel temp;
        public PrzegladanieGrupViewModel()
        {
            przegladanieGrupViewModel = this;
            
            temp = this;
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                Items = uw.GrupyZajeciowe.GetGrupyZajecioweWithRelations().ToList();
            }
        }
        public void przegladanieFiltrowanie(String potok)
        {
            ItemsChanged = new List<GrupyZajeciowe>();
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                Items = uw.GrupyZajeciowe.GetGrupyZajecioweWithRelations().ToList();
            }
                foreach(GrupyZajeciowe gz in Items)
                {
                    if(gz.Potok.Equals(potok))
                    {
                        ItemsChanged.Add(gz);
                    }
                }
            Items.Clear();
            Items = new List<GrupyZajeciowe>();
            foreach(GrupyZajeciowe gz in ItemsChanged)
            {
                Items.Add(gz);
            }
            NotifyPropertyChange("Items");
        }
        public void PrzegladanieProwadzacy(String Nazwisko)
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                
                List<Prowadzacy> pro=uw.Prowadzacy.GetAll().ToList();
                System.Diagnostics.Debug.WriteLine(Items.Count);
                List<long?> idP = new List<long?>();
                for(int i=0;i<pro.Count();i++)
                {
                    if(pro.ElementAt(i).Nazwisko.StartsWith(Nazwisko, StringComparison.OrdinalIgnoreCase))
                        idP.Add(pro.ElementAt(i).IdProwadzacego);
                }
                List<GrupyZajeciowe> temp = uw.GrupyZajeciowe.GetGrupyZajecioweWithRelations().ToList();
                Items = new List<GrupyZajeciowe>();
               for(int i=0; i<temp.Count();i++)
                {
                    if (idP.Contains(temp.ElementAt(i).IdProwadzacego))
                        Items.Add(temp.ElementAt(i));
                    System.Diagnostics.Debug.WriteLine(Items.Count);
                }
                NotifyPropertyChange("Items");


            }

        }
        public void FiltrowanieGrup(String potok, String nazwisko, String kodGrupy, String nazwaKursu, String kodKursu)
        {
            ItemsChanged = new List<GrupyZajeciowe>();
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                List<Prowadzacy> prowadzacy = uw.Prowadzacy.GetAll().ToList();
                Items = uw.GrupyZajeciowe.GetGrupyZajecioweWithRelations().ToList();
            }
            foreach(GrupyZajeciowe gz in Items)
            {
            }
        }

        public void filtrujNazwaKursu(String nazwaKursu, String potok, String kodGrupy, String kodKursu, String prowadzacy)
        {
            ItemsChanged = new List<GrupyZajeciowe>();
            List<Kursy> kursy = new List<Kursy>();
            List<Prowadzacy> prowadzacyList = new List<Prowadzacy>();
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                Items = uw.GrupyZajeciowe.GetAll().ToList();
                kursy = uw.Kursy.GetAll().ToList();
            }
            if(nazwaKursu.Equals("") && potok.Equals("") && kodGrupy.Equals("") && kodKursu.Equals(""))
            {
                foreach (GrupyZajeciowe gz in Items)
                    ItemsChanged.Add(gz);
            }
            foreach(GrupyZajeciowe gz in Items)
            {
                foreach(Kursy kr in kursy)
                {
                    if (kr.NazwaKursu.IndexOf(nazwaKursu, StringComparison.CurrentCultureIgnoreCase) >=0 && gz.KodKursu==kr.KodKursu && gz.Potok.StartsWith(potok, StringComparison.OrdinalIgnoreCase) && gz.KodGrupy.StartsWith(kodGrupy, StringComparison.OrdinalIgnoreCase) && gz.KodKursu.StartsWith(kodKursu, StringComparison.OrdinalIgnoreCase))
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
       
    }
}
