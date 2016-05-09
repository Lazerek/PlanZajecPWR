using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanZajec.ViewModels
{
    public class GrupaTamplateViewModel
    {
        public GrupyZajeciowe GrupZaj
        {
            get; set;
        }
        public string nazwa;

        public GrupaTamplateViewModel(string Kod)
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
               var grupa = uw.GrupyZajeciowe.Get(Kod);
               GrupZaj = grupa;
               nazwa = grupa.Kursy.NazwaKursu;
                //ListaGrupZajeciowych[0];
            }
        }
    }
}
