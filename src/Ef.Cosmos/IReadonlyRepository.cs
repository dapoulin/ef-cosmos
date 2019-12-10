using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ef.Cosmos
{
    public interface IReadonlyRepository<TEntity> where TEntity : class, IEntity
    {
        IQueryable<TEntity> List();
        Task<TEntity> GetById(Guid id);
        Task<TEntity> GetOne(Expression<Func<TEntity, bool>> predicate);
    }
}