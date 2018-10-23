using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SerializationDemos
{
    // Required for Binary serialization.
    [Serializable]
    // Required for DataContract serialization.
    [DataContract]
    public class Dog : Animal
    {
        public Dog(string name = null)
            : base(name)
        {
        }
    }
}
