using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

namespace Ef.Cosmos.Samples.ConsoleAppDI
{
    class Program
    {
        static void Main(string[] args)
        {
            Startup.Init();
            Console.WriteLine("Hello World!");
        }

        static async Task Run()
        {

            var uow = Startup.ServiceProvider.GetService<IUnitOfWork>() ;
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