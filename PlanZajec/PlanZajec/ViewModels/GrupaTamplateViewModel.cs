using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanZajec.ViewModels
{
    class GrupaTamplateViewModel
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
               var ListaGrupZajeciowych = uw.Plany.Get(1).GrupyZajeciowe.ToList();
               foreach(GrupyZajeciowe gr in ListaGrupZajeciowych)
                {
                    if (gr.KodGrupy.Equals(Kod))
                    {
                        GrupZaj = gr;
                        nazwa = gr.Kursy.NazwaKursu;
                    }
                }
                //ListaGrupZajeciowych[0];
            }
        }
    }
}
