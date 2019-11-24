using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Threading.Tasks;

namespace Ef.Cosmos.Samples.ConsoleAppNoDI
{
    class Program
    {
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

        static void Main(string[] args)
        {
            Console.WriteLine("Hello Cosmos !");

            Run().Wait();
        }

        static async Task Run()
        {
            var settings = new DbSettings();

            var ctx = new PersonContext(settings);
            await ctx.Database.EnsureCreatedAsync();

            var uow = new UnitOfWork<CosmosDbContext>(ctx);
            var repo = uow.GetRepository<Person>();

            var david = new Person { PersonId = Guid.NewGuid(), Name = "David", Age = 37 };
            var christelle = new Person { PersonId = Guid.NewGuid(), Name = "Christelle", Age = 36 };

            await repo.Create(david);
            await repo.Create(christelle);
            await uow.SaveChangesAsync();

            var people = repo.List();
            foreach (var person in people)
            {
                Console.WriteLine($"There is {person.Name} / {person.Age} yo");
            }
        }        
    }
}