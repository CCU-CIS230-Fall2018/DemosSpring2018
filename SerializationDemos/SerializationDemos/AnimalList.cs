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
    [KnownType(typeof(Cat))]
    [KnownType(typeof(Dog))]
    public class AnimalList<T> : List<T> where T : Animal
    {
    }
}
