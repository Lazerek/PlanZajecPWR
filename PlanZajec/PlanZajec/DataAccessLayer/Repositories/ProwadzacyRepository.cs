using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanZajec.DataModel;
using PlanZajec.DataAccessLayer.IRepositories;

namespace PlanZajec.DataAccessLayer.Repositories
{
    public class ProwadzacyRepository : Repository<Prowadzacy>, IProwadzacyRepository
    {
        public ProwadzacyRepository(PlanPwrContext context) : base(context)
        {
        }

        public PlanPwrContext PlanPwrContext
        {
            get { return Context as PlanPwrContext; }
        }

    }
}
