﻿using System;
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
            return DodajGrupeZajeciowaDoPlanu(grupa, ActChosenPlanSingleton.Instance.IdPlanu);
        }

        public bool DodajGrupeZajeciowaDoPlanu(GrupyZajeciowe grupa, long idPlanu)
        {
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
            // Here we are working with a DbContext, not PlutoContext. So we don't have DbSets 
            // such as Courses or Authors, and we need to use the generic Set() method to access them.
            return PlanPwrContext.GrupyZajeciowe.Where(g => g.Plany.Any(pl => pl.IdPlanu == id))
                .Include(g => g.Prowadzacy)
                .Include(g => g.Kursy); //. GrupyZajeciowe.;
        }



        
    }
}
