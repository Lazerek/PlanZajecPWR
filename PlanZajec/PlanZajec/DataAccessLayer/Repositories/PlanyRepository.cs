﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanZajec.DataModel;
using PlanZajec.DataAccessLayer.IRepositories;

namespace PlanZajec.DataAccessLayer.Repositories
{
    public class PlanyRepository : Repository<Plany>, IPlanyRepository
    {
        public PlanyRepository(PlanPwrContext context) : base(context)
        {
        }

        public PlanPwrContext PlanPwrContext
        {
            get { return Context as PlanPwrContext; }
        }
    }
}