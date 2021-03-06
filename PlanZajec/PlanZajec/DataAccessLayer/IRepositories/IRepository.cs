using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PlanZajec.DataAccessLayer.IRepositories
{
    public interface IRepository<TEntity, Tkey> where TEntity : class
    {
        TEntity Get(Tkey id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}