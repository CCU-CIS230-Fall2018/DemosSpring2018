using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValueReferenceTypes
{
    class Kite : IFly
    {
        public void Fly()
        {
            Console.WriteLine("Gliding");
        }
    }
}
