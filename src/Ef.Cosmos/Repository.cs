using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Ef.Cosmos
{
    public class Repository<TEntity> :
       ReadonlyRepository<TEntity>,
       IRepository<TEntity> where TEntity : class
    {
        public Repository(DbContext unitOfWork) : base(unitOfWork)
        {
        }

        public async Task Create(TEntity entity)
        {
            await TargetDbSet.AddAsync(entity);
        }
        public Task Update(TEntity entity)
        {
            return Task.Run(() => TargetDbSet.Update(entity));
        }
        public async Task Delete(Guid id)
        {
            var entity = await GetById(id).ConfigureAwait(false);
            TargetDbSet.Remove(entity);
        }

        public Task Delete(TEntity entity)
        {
            return Task.Run(() => TargetDbSet.Remove(entity));
        }
    }
}
