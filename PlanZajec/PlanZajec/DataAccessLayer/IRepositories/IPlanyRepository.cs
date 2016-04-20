using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanZajec.DataModel;

namespace PlanZajec.DataAccessLayer.IRepositories
{
    public interface IPlanyRepository : IRepository<Plany>
    {
        bool DodajGrupeZajeciowaDoPlanu(GrupyZajeciowe grupa);

        bool UsunGrupeZajeciowaZPlanu(GrupyZajeciowe grupa);

        //bool DodajGrupeZajeciowaDoPlanu(GrupyZajeciowe grupa, Plany plan);
        //bool UsunGrupeZajeciowaZPlanu(GrupyZajeciowe grupa, Plany plan);

        
        int Count();

        Plany GetFirstOrDefault();
    }
}
