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

    public class PrzegladanieGrupView : ViewModel
    {
        public List<GrupyZajeciowe> Items { get; set; }
        public PrzegladanieGrupView()
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                Items = uw.GrupyZajeciowe.GetAll().ToList();
                for(int i=0;i<Items.Count();i++)
                {
                    System.Diagnostics.Debug.WriteLine(Items);
                    
                }
                

            }
        }
       
    }
}
