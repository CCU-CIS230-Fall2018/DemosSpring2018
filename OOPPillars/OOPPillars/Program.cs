using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPPillars
{
    class Program
    {
        static void Main(string[] args)
        {
            Animal fred = new Mammal();
            fred.NumberOfAppendages = 4;

            Console.WriteLine(fred.NumberOfAppendages);
        }
    }
}
