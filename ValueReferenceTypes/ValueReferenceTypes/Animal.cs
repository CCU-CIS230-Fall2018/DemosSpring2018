using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValueReferenceTypes
{
    class Animal
    {
        public Animal()
        {
            Console.WriteLine("I am an animal!");
        }

        public string Name { get; set; }
    }
}
