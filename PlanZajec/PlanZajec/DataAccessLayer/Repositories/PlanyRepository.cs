using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanZajec.DataModel;
using PlanZajec.DataAccessLayer.IRepositories;
using PlanZajec.CommonInformations;

using System.Data.Entity;

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
            if(!PlanPwrContext.Set<GrupyZajeciowe>().Local.Any(e => e.KodGrupy == grupa.KodGrupy))
            {
                PlanPwrContext.GrupyZajeciowe.Attach(grupa);
            }
            if (!PlanPwrContext.Set<Plany>().Local.Any(e => e.IdPlanu == plan.IdPlanu))
            {
                PlanPwrContext.Plany.Attach(plan);
            }
            
            bool result = false;
            if(!grupa.Plany.Any(pl => pl.IdPlanu == plan.IdPlanu))
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
            if (!PlanPwrContext.Set<GrupyZajeciowe>().Local.Any(e => e.KodGrupy == grupa.KodGrupy))
            {
                PlanPwrContext.GrupyZajeciowe.Attach(grupa);
            }
            if (!PlanPwrContext.Set<Plany>().Local.Any(e => e.IdPlanu == plan.IdPlanu))
            {
                PlanPwrContext.Plany.Attach(plan);
            }
            bool result = false;
            if (grupa.Plany.Any(pl => pl.IdPlanu == plan.IdPlanu))
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


        //TODO: Greg
        public IEnumerable<GrupyZajeciowe> GetGrupyZajecioweWithRelation(long id)
        {
            // Here we are working with a DbContext, not PlutoContext. So we don't have DbSets 
            // such as Courses or Authors, and we need to use the generic Set() method to access them.
            return PlanPwrContext.GrupyZajeciowe.Where(g => g.Plany.Any(pl => pl.IdPlanu == id))
                .Include(g => g.Prowadzacy)
                .Include(g => g.Kursy); //. GrupyZajeciowe.;
        }

    }
}
