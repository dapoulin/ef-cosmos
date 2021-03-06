﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ef.Cosmos
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity;
        IReadonlyRepository<TEntity> GetReadonlyRepository<TEntity>() where TEntity : class, IEntity;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        TContext Context { get; }
    }
}