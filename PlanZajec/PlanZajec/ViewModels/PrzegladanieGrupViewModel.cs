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
        public List<GrupyZajeciowe> ItemsChanged { get; set; }
        public static PrzegladanieGrupViewModel temp;
        public PrzegladanieGrupViewModel()
        {
            temp = this;
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                Items = uw.GrupyZajeciowe.GetAll().ToList();
            }
        }
        public void przegladanieFiltrowanie(String potok)
        {
            ItemsChanged = new List<GrupyZajeciowe>();
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                Items = uw.GrupyZajeciowe.GetAll().ToList();
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
       
    }
}
