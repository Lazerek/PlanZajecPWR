using PlanZajec.DataAccessLayer.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanZajec.DataAccessLayer
{
    public interface IUnitOfWork : IDisposable
    {
        IBlokRepository Bloki { get; }
        IGrupyZajecioweRepository GrupyZajeciowe { get; }
        IKursyRepository Kursy { get; }
        IPlanyRepository Plany { get; }
        IProwadzacyRepository Prowadzacy { get; }

        int SaveChanges();
    }


}
