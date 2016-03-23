

using System;
using PlanZajec.DataAccessLayer.IRepositories;
using PlanZajec.DataModel;
using PlanZajec.DataAccessLayer.Repositories;

namespace PlanZajec.DataAccessLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PlanPwrContext _context;

        public UnitOfWork(PlanPwrContext context)
        {
            _context = context;
            Bloki =         new BlokRepository(_context);
            GrupyZajeciowe =new GrupyZajecioweRepository(_context);
            Kursy =         new KursyRepository(_context);
            Plany =         new PlanyRepository(_context);
            Prowadzacy =    new ProwadzacyRepository(_context);

        }

        public IBlokRepository Bloki { get; private set; }

        public IGrupyZajecioweRepository GrupyZajeciowe { get; private set; }

        public IKursyRepository Kursy { get; private set; }

        public IPlanyRepository Plany { get; private set; }

        public IProwadzacyRepository Prowadzacy { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}