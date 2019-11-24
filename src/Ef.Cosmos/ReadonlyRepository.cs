using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ef.Cosmos
{
    public class ReadonlyRepository<TEntity> :
        IReadonlyRepository<TEntity> where TEntity : class
    {
        public DbContext Context { get; private set; }
        public DbSet<TEntity> TargetDbSet { get; private set; }

        public ReadonlyRepository(DbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(DbContext));
            TargetDbSet = Context.Set<TEntity>();
        }

        public virtual Task<TEntity> GetById(Guid id) => throw new NotImplementedException();

        public Task<TEntity> GetOne(Expression<Func<TEntity, bool>> predicate) => TargetDbSet.FirstOrDefaultAsync(predicate);

        public IQueryable<TEntity> List() => TargetDbSet;
    }
}
