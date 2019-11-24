using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;

namespace Ef.Cosmos.Samples.ConsoleAppDI
{
    class Startup
    {
        public static IConfiguration Configuration { get; private set; }
        public static ServiceProvider ServiceProvider { get; private set; }

        public static void Init()
        {
            ConfigureConfiguration();
            ConfigureLogging(Configuration);

            var servicesCollection = new ServiceCollection();
            ConfigureServices(servicesCollection);

            ServiceProvider = servicesCollection.BuildServiceProvider();

            InitDbContext();
        }

        static void InitDbContext()
        {
            var dbContext = ServiceProvider.GetService<PersonContext>();
            dbContext.Database.EnsureCreated();
        }

        static void ConfigureLogging(IConfiguration configuration = null)
        {
            Log.Logger = new LoggerConfiguration()
                 .MinimumLevel.Information()
                 .WriteTo.Console()
                 .CreateLogger();
        }

        static void ConfigureConfiguration()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }

        static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(config => config.AddSerilog());
            services.AddOptions();

            services.AddSingleton<IConfiguration>(Configuration);

            services.AddSingleton<IDbSettings, DbSettings>();

            services.AddDbContext<PersonContext>();
            services.AddTransient<IUnitOfWork, UnitOfWork<PersonContext>>();
        }
    }


    //
    // 1 - define your database settings
    //
    public class DbSettings : IDbSettings
    {
        public DbSettings()
        {
            //Azure Cosmos Db Emulator Settings
            Uri = new Uri("https://localhost:8081");
            PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
            DatabaseName = "Universe";
            EnableDetailedErrors = true;
            EnableSensitiveDataLogging = true;
        }
        public Uri Uri { get; set; }
        public string PrimaryKey { get; set; }
        public string DatabaseName { get; set; }
        public bool EnableDetailedErrors { get; set; }
        public bool EnableSensitiveDataLogging { get; set; }
    }

    //
    // 2 - Get Your model ready
    // Primary keys for your entities work better with YourEntityName + Id (below, PersonId)
    public class Person
    {
        public Guid PersonId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }

    //
    // 3 - Create your DbContext // override OnModelCreating to init your entities
    //
    public class PersonContext : CosmosDbContext
    {
        public PersonContext(IDbSettings settings) : base(settings) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Person>()
            .Property(p => p.PersonId)
            .HasValueGenerator<GuidValueGenerator>();
        }
    }
}
