using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanZajec.DataModel;
using PlanZajec.DataAccessLayer.IRepositories;

namespace PlanZajec.DataAccessLayer.Repositories
{
    public class BlokRepository : Repository<Blok, string>, IBlokRepository
    {
        public BlokRepository(PlanPwrContext context) : base(context)
        {
        }

        public PlanPwrContext PlanPwrContext
        {
            get { return Context as PlanPwrContext; }
        }


    }
}
