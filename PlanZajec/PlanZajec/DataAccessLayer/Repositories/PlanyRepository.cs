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
    public class PlanyRepository : Repository<Plany, long>, IPlanyRepository
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
            return DodajGrupeZajeciowaDoPlanu(grupa, ActChosenPlanSingleton.Instance.IdPlanu);
        }

        public bool DodajGrupeZajeciowaDoPlanu(GrupyZajeciowe grupa, long idPlanu)
        {
            if (idPlanu < 0)
                return false;
            if(!PlanPwrContext.Set<GrupyZajeciowe>().Local.Any(e => e.KodGrupy == grupa.KodGrupy))
            {
                PlanPwrContext.GrupyZajeciowe.Attach(grupa);
            }
            Plany planWybrany = PlanPwrContext.Plany.Include(pl => pl.GrupyZajeciowe).FirstOrDefault(pl => pl.IdPlanu == idPlanu);

            bool result = false;
            if(!grupa.Plany.Any(pl => pl.IdPlanu == idPlanu))
            {
                planWybrany.GrupyZajeciowe.Add(grupa);
                Context.SaveChanges();
                result = true;
            }
            return result;
        }

        public bool UsunGrupeZajeciowaZPlanu(GrupyZajeciowe grupa)
        {
            return UsunGrupeZajeciowaZPlanu(grupa, ActChosenPlanSingleton.Instance.IdPlanu);
        }

        public bool UsunGrupeZajeciowaZPlanu(GrupyZajeciowe grupa, long idPlanu)
        {
            if (!PlanPwrContext.Set<GrupyZajeciowe>().Local.Any(e => e.KodGrupy.Equals(grupa.KodGrupy)))
            {
                PlanPwrContext.GrupyZajeciowe.Attach(grupa);
            }

            Plany planWybrany = PlanPwrContext.Plany.Include(pl => pl.GrupyZajeciowe).FirstOrDefault(pl => pl.IdPlanu == idPlanu);

            bool result = false;
            if (grupa.Plany.Any(pl => pl.IdPlanu == idPlanu))
            {
                planWybrany.GrupyZajeciowe.Remove(grupa);
                Context.SaveChanges();
                result = true;
            }
            return result;
        }



        public Plany GetFirstOrDefault()
        {
            return PlanPwrContext.Plany.Include(pl => pl.GrupyZajeciowe).FirstOrDefault();
        }


        //TODO: Greg
        public IEnumerable<GrupyZajeciowe> GetGrupyZajecioweWithRelation(long id)
        {
            return PlanPwrContext.GrupyZajeciowe.Where(g => g.Plany.Any(pl => pl.IdPlanu == id))
                .Include(g => g.Prowadzacy)
                .Include(g => g.Kursy); //. GrupyZajeciowe.;
        }

        public override void Remove(Plany entity)
        {
            Plany planDoUsuniecia = PlanPwrContext.Plany.Include(pl => pl.GrupyZajeciowe).
                SingleOrDefault(pl => pl.IdPlanu == entity.IdPlanu);
            if(planDoUsuniecia != null)
            {
                foreach(GrupyZajeciowe grupa in planDoUsuniecia.GrupyZajeciowe.ToList())
                {
                    planDoUsuniecia.GrupyZajeciowe.Remove(grupa);
                }
                Context.Set<Plany>().Remove(planDoUsuniecia);
            }
        }
        public override IEnumerable<Plany> GetAll()
        {
            return PlanPwrContext.Plany.Include(pl => pl.GrupyZajeciowe).ToList();
        }

    }
}
