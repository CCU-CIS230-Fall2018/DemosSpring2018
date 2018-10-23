using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValueReferenceTypes
{
    class Program
    {
        static void Main(string[] args)
        {
            Animal bob1 = new Animal { Name = "bob" };
            Animal bob2 = bob1; // new Animal { Name = "bob" };
            bob2.Name = "bob2";
            Console.WriteLine(bob1 == bob2);
            Console.WriteLine(bob1.Name);

            int x = 5;
            int y = 6;
            int z = x;
            z = 43;
            Console.WriteLine($"x={x},y={y},z={z}");

            string string1 = "bob";
            string string2 = string1;
            string2 = "john";
            Console.WriteLine($"{string1},{string2}");
            Console.WriteLine(string1 == string2);

            IFly flyer = new Kite();
            IFly flyer2 = new Bird("bluejay");

            flyer.Fly();
            flyer2.Fly();
        }
    }
}
