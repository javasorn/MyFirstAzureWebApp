using DbUp;
using System;
using System.Linq;
using System.Reflection;

namespace Database.Bootstrap
{
    class Program
    {
        // DbUp
        // https://dbup.readthedocs.io/en/latest/

        static void Main(string[] args)
        {
            var connectionString = args.FirstOrDefault()
                    ?? "Initial Catalog = MyApp; Data Source =.; Integrated Security = True; Pooling = true";
            
            // If you want your application to create the database for you, add the following line after the connection string:
            EnsureDatabase.For.SqlDatabase(connectionString);

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
        }
    }
}
