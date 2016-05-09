using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanZajec.DataModel;

namespace PlanZajec.DataAccessLayer.IRepositories
{
    public interface IPlanyRepository : IRepository<Plany, long>
    {
        bool DodajGrupeZajeciowaDoPlanu(GrupyZajeciowe grupa);
        bool DodajGrupeZajeciowaDoPlanu(GrupyZajeciowe grupa, long idPlanu);

        bool UsunGrupeZajeciowaZPlanu(GrupyZajeciowe grupa);
        bool UsunGrupeZajeciowaZPlanu(GrupyZajeciowe grupa, long idPlanu);

        int Count();

        Plany GetFirstOrDefault();
    }
}
