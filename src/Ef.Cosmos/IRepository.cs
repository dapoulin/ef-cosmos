using System;
using System.Threading.Tasks;

namespace Ef.Cosmos
{
    public interface IRepository<TEntity> : IReadonlyRepository<TEntity> where TEntity : class, IEntity
    {
        Task Create(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(Guid id);
        Task Delete(TEntity entity);
    }
}
