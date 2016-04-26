using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanZajec.ViewModels
{
    class PlanViewModel
    {
        public List<GrupyZajeciowe> ListaGrupZajeciowych { get; private set; }

        public PlanViewModel()
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                ListaGrupZajeciowych = uw.Plany.Get(1).GrupyZajeciowe.ToList();
               
                //ListaGrupZajeciowych[0];
            }
        }
    }
}
