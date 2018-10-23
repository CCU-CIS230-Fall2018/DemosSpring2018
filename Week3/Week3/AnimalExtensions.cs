using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3
{
    static class AnimalExtensions
    {
        public static void Butcher(this Animal a)
        {
            Console.WriteLine("Nooooooo!");
        }
    }
}
