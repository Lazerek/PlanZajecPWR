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

        public PrzegladanieGrupViewModel()
        {
            przegladanieGrupViewModel = this;
            
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                Items = uw.GrupyZajeciowe.GetAll().ToList();
                
                

            }
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
                    if(pro.ElementAt(i).Nazwisko.Equals(Nazwisko))
                        idP.Add(pro.ElementAt(i).IdProwadzacego);
                }
                List<GrupyZajeciowe> temp = uw.GrupyZajeciowe.GetAll().ToList();
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
       
    }
}
