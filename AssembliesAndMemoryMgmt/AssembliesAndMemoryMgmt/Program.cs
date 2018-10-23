using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AssembliesAndMemoryMgmt
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //WriteAssemblyInfo();

            ManageContacts();
        }

        private  static void WriteAssemblyInfo()
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            Console.WriteLine("FullName: {0}", currentAssembly.FullName);
            Console.WriteLine("Location: {0}", currentAssembly.Location);
            Console.WriteLine("CodeBase: {0}", currentAssembly.CodeBase);

            Console.WriteLine();
            Console.WriteLine("Modules:");
            foreach (Module module in currentAssembly.Modules)
            {
                Console.WriteLine("Module FQN: {0}", module.FullyQualifiedName);
            }

            Console.WriteLine();
            Console.WriteLine("Attributes:");
            foreach (var attribute in currentAssembly.CustomAttributes)
            {
                Console.WriteLine(attribute);
            }

            Console.WriteLine();
            Console.WriteLine("Types:");
            foreach (var type in currentAssembly.GetTypes())
            {
                Console.WriteLine(type);
            }
        }

        private static void ManageContacts()
        {
            const int maxIterations = 10000;
            const int maxContactsPerIteration = 10;

            var repository = new ContactRepository();

            for (int i = 0; i < maxIterations; i++)
            {
                // Create a new contact manager.
                using (ContactManager manager = new ContactManager(repository))
                {
                    // Add some new contacts to the repository.
                    for (int j = 0; j < maxContactsPerIteration; j++)
                    {
                        manager.Add(new Contact { Name = "Contact " + j });
                    }

                    //manager = null;

                    //Console.WriteLine(manager.Count);
                }

                // Display the memory used after forcing garbage collection.
                Console.WriteLine("Memory Used: {0:n0} bytes", GC.GetTotalMemory(true));
            }
        }
    }
}
