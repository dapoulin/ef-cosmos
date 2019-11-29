using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ef.Cosmos
{
    public class UnitOfWork<TContext> :
        IUnitOfWork<TContext>, IUnitOfWork
        where TContext : DbContext, IDisposable
    {
        private bool disposed;

        private Dictionary<Type, object> readonlyRepositories;
        private Dictionary<Type, object> repositories;

        public TContext Context { get; }

        public UnitOfWork(TContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context)); ;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposning)
        {
            if (disposed)
                return;

            if (disposning)
            {
                Context?.Dispose();
            }

            disposed = true;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return Context.SaveChangesAsync();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity
        {
            if (repositories == null) repositories = new Dictionary<Type, object>();

            var type = typeof(TEntity);
            if (!repositories.ContainsKey(type)) repositories[type] = new Repository<TEntity>(Context);
            return (IRepository<TEntity>)repositories[type];
        }

        public IReadonlyRepository<TEntity> GetReadonlyRepository<TEntity>() where TEntity : class, IEntity
        {
            if (readonlyRepositories == null) readonlyRepositories = new Dictionary<Type, object>();

            var type = typeof(TEntity);
            if (!readonlyRepositories.ContainsKey(type)) readonlyRepositories[type] = new ReadonlyRepository<TEntity>(Context);
            return (IReadonlyRepository<TEntity>)readonlyRepositories[type];
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
