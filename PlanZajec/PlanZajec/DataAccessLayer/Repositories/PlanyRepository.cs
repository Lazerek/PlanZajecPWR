using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanZajec.DataModel;
using PlanZajec.DataAccessLayer.IRepositories;
using PlanZajec.CommonInformations;

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

        public int Count()
        {
            return PlanPwrContext.Plany.Count();
        }

        public bool DodajGrupeZajeciowaDoPlanu(GrupyZajeciowe grupa)
        {
            return DodajGrupeZajeciowaDoPlanu(grupa, ActChosenPlanSingleton.Instance.Plan);
        }

        public bool DodajGrupeZajeciowaDoPlanu(GrupyZajeciowe grupa, Plany plan)
        {
            PlanPwrContext.GrupyZajeciowe.Attach(grupa);
            bool result = false;
            if(!grupa.Plany.Contains(plan))
            {
                plan.GrupyZajeciowe.Add(grupa);
                Context.SaveChanges();
                result = true;
            }
            return result;
        }

        public bool UsunGrupeZajeciowaZPlanu(GrupyZajeciowe grupa)
        {
            return UsunGrupeZajeciowaZPlanu(grupa, ActChosenPlanSingleton.Instance.Plan);
        }

        public bool UsunGrupeZajeciowaZPlanu(GrupyZajeciowe grupa, Plany plan)
        {
            PlanPwrContext.GrupyZajeciowe.Attach(grupa);
            bool result = false;
            if (grupa.Plany.Contains(plan))
            {
                plan.GrupyZajeciowe.Remove(grupa);
                Context.SaveChanges();
                result = true;
            }
            return result;
        }



        public Plany GetFirstOrDefault()
        {
            return PlanPwrContext.Plany.FirstOrDefault();
        }
    }
}
