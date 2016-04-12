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

        public IEnumerable<GrupyZajeciowe> GetGrupyZajecioweWithRelations()
        {
            return PlanPwrContext.GrupyZajeciowe
                .Include(g => g.Prowadzacy)
                .Include(g => g.Kursy)
                .ToList();
        }



    }
}
