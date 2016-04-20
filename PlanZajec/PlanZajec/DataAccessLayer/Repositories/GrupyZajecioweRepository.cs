using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanZajec.DataModel;
using PlanZajec.DataAccessLayer.IRepositories;

using System.Data.Entity; //Ważne!


namespace PlanZajec.DataAccessLayer.Repositories
{
    public class GrupyZajecioweRepository : Repository<GrupyZajeciowe>, IGrupyZajecioweRepository
    {
        public GrupyZajecioweRepository(PlanPwrContext context) : base(context)
        {
        }

        public PlanPwrContext PlanPwrContext
        {
            get { return Context as PlanPwrContext; }
        }

        public override GrupyZajeciowe Get(long id)
        {
            return null;
        }

        public GrupyZajeciowe Get(string kodGrupy)
        {
            return PlanPwrContext.GrupyZajeciowe.FirstOrDefault(g => g.KodGrupy.Equals(kodGrupy));
        }

        public IEnumerable<GrupyZajeciowe> GetGrupyZajecioweWithRelations()
        {
            return PlanPwrContext.GrupyZajeciowe
                .Include(g => g.Prowadzacy)
                .Include(g => g.Kursy)
                .ToList();
        }





    }
}
