using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValueReferenceTypes
{
    class Bird : Animal, IFly
    {
        public Bird()
        {
            Console.WriteLine("I am a bird!");
        }

        public Bird(string type = null)
            : this()
        {
            Console.WriteLine($"I am a {type}.");
        }

        public void Fly()
        {
            Console.WriteLine("Flap, flap");
        }
    }
}
