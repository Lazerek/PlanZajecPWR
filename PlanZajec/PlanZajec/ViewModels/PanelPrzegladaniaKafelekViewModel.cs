using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanZajec.ViewModels
{
    //TODO GREG DELETE THIS SHIT CAUSE U DONT USE THIS
    //TODO GREG DELETE THIS SHIT CAUSE U DONT USE THIS
    //TODO GREG DELETE THIS SHIT CAUSE U DONT USE THIS
    //TODO GREG DELETE THIS SHIT CAUSE U DONT USE THIS
    class PanelPrzegladaniaKafelekViewModel
    {
        public List<GrupyZajeciowe> ListaGrupZajeciowych { get; private set; }

        public PanelPrzegladaniaKafelekViewModel()
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                ListaGrupZajeciowych = uw.GrupyZajeciowe.GetGrupyZajecioweWithRelations().ToList();
                //ListaGrupZajeciowych[0];
            }
        }

    }
}
