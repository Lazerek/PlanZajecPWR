﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanZajec.DataModel;

namespace PlanZajec.DataAccessLayer.IRepositories
{
    public interface IGrupyZajecioweRepository : IRepository<GrupyZajeciowe, string>
    {
        IEnumerable<GrupyZajeciowe> GetGrupyZajecioweWithRelations();

        GrupyZajeciowe Get(string kodGrupy);
        bool InThePlan(string kodGrupy, long idPlanu);

    }
}
