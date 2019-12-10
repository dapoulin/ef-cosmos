using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Ef.Cosmos
{
    public abstract class CosmosDbContext : DbContext
    {
        readonly ILoggerFactory loggerFactory;
        readonly IDbSettings dbSettings;

        public CosmosDbContext(ILoggerFactory loggerFactory, IDbSettings settings)
        {
            this.loggerFactory = loggerFactory;
            this.dbSettings = settings ?? throw new ArgumentNullException(nameof(IDbSettings));
        }

        public CosmosDbContext(IDbSettings settings) : this(null, settings) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseCosmos(dbSettings.Uri.ToString(), dbSettings.PrimaryKey, dbSettings.DatabaseName);
            if (loggerFactory != null)
            {
                optionsBuilder.UseLoggerFactory(loggerFactory);
            }
            optionsBuilder.EnableDetailedErrors(dbSettings.EnableDetailedErrors);
            optionsBuilder.EnableSensitiveDataLogging(dbSettings.EnableSensitiveDataLogging);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            if(!string.IsNullOrEmpty(dbSettings.ContainerName))
            {
                modelBuilder.HasDefaultContainer(dbSettings.ContainerName);
            }
        }
    }
}
