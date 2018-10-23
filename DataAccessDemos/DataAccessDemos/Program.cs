using EntityModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessDemos
{
    class Program
    {
        public const string ConnectionString = "Data Source=.;Initial Catalog=AdventureWorks2014;Integrated Security=True";

        static void Main(string[] args)
        {
            SqlClientQuery();

            //CommonClientQuery();

            //SqlDataSetQuery();

            //EntityQuery();

            //EntityCrud();

            //LinqFluentSyntax();

            //LinqQuerySyntax();
        }

        private static void LinqFluentSyntax()
        {
            List<string> names = new List<string> { "Monkey", "Bear", "Dog", "Cat" };
            var query = names.Where(n => n.Contains("B"));

            foreach (string s in query)
            {
                Console.WriteLine(s);
            }
        }

        private static void LinqQuerySyntax()
        {
            List<string> names = new List<string> { "Monkey", "Bear", "Dog", "Cat" };
            var namesWithO = (from n in names where n.Contains("o") select n).Where(s => s.Length > 3);

            names.Add("Frog");

            foreach (string s in namesWithO)
            {
                Console.WriteLine(s);
            }
        }

        private static void SqlClientQuery(string name = "Bob")
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand command = new SqlCommand($"SELECT TOP 10 FirstName, LastName FROM Person.Person WHERE FirstName = @NAME", connection))
            {
                //name = "' delete database; --";
                connection.Open();
                

                command.Parameters.AddWithValue("NAME", name);

                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    int firstNameOrdinal = 0;  reader.GetOrdinal("FirstName");
                    int lastNameOrdinal = 1;  reader.GetOrdinal("LastName");

                    while (reader.Read())
                    {
                        string firstName = reader.GetString(firstNameOrdinal);
                        string lastName = reader.GetString(lastNameOrdinal);

                        Console.WriteLine("{0}, {1}", lastName, firstName);
                    }
                }

                //connection.Close();
            }
        }

        private static void CommonClientQuery()
        {
            DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.SqlClient");

            using (DbConnection connection = factory.CreateConnection())
            using (DbCommand command = connection.CreateCommand())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                command.CommandText = "SELECT TOP 10 * FROM Person.Person WHERE FirstName = @P1";

                var parameter = command.CreateParameter();
                parameter.ParameterName = "P1";
                parameter.Value = "Bob";
                command.Parameters.Add(parameter);

                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    int firstNameOrdinal = reader.GetOrdinal("FirstName");
                    int lastNameOrdinal = reader.GetOrdinal("LastName");

                    while (reader.Read())
                    {
                        string firstName = reader.GetString(firstNameOrdinal);
                        string lastName = reader.GetString(lastNameOrdinal);

                        Console.WriteLine("{0}, {1}", lastName, firstName);
                    }
                }
            }
        }

        private static void SqlDataSetQuery()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT TOP 10 * FROM Person.Person WHERE FirstName = @P1", connection))
            {
                command.Parameters.AddWithValue("P1", "Bob");

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataSet bobs = new DataSet();
                adapter.Fill(bobs, "Person");

                foreach (DataRow row in bobs.Tables["Person"].Rows)
                {
                    Console.WriteLine("{0}, {1}", row["LastName"], row["FirstName"]);
                }
            }
        }

        private static void EntityQuery()
        {
            AdventureWorksEdm context = new AdventureWorksEdm();
            var people = context.People.Select(p => new { p.FirstName, p.LastName })
                .Where(p => p.FirstName == "Bob")
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .Take(5);

            foreach (var person in people)
            {
                Console.WriteLine("{0}, {1}", person.LastName, person.FirstName);
            }
        }

        private static bool IsBob(Person p)
        {
            return p.FirstName == "Bob";
        }

        private static void EntityCrud()
        {
            using (AdventureWorksEdm context = new AdventureWorksEdm())
            {
                var mattQuery = context.People.Where(p => p.FirstName == "Matt" && p.LastName == "Anderson");

                // Show that Matt does not exist in the database.
                Console.WriteLine("Matt's ID is {0}", mattQuery
                    .FirstOrDefault()?
                    .BusinessEntityID
                    .ToString() ?? "<null>");

                var person = new Person
                {
                    BusinessEntity = new BusinessEntity { rowguid = Guid.NewGuid(), ModifiedDate = DateTime.Now },
                    FirstName = "Matt",
                    LastName = "Anderson",
                    PersonType = "EM",
                    rowguid = Guid.NewGuid(),
                    ModifiedDate = DateTime.Now
                };

                // Create a new person.
                context.People.Add(person);
                context.SaveChanges();

                // Read the person.
                var matt = mattQuery.First();
                Console.WriteLine("Matt's ID is {0}", matt.BusinessEntityID.ToString() ?? "<null>");

                // Update the person.
                matt.MiddleName = "Douglas";
                context.SaveChanges();

                // Read the new person.
                Console.WriteLine("Matt's middle name is {0}", matt.MiddleName ?? "<null>");

                // Delete the person.
                context.People.Remove(matt);
                context.SaveChanges();

                // Show that Matt does not exist in the database.
                Console.WriteLine("Matt's ID is {0}", mattQuery.FirstOrDefault()?.BusinessEntityID.ToString() ?? "<null>");
            }
        }
    }
}
