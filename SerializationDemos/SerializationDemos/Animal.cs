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
    public class Animal
    {
        public Animal(string name = null)
        {
            Name = name;

            Console.WriteLine("A {0} named {1} was constructed.", this.GetType().Name, name ?? "<null>");
        }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember]
        public int NumberOfLegs { get; set; }

        [DataMember]
        //[IgnoreDataMember]
        public DateTime? BirthDate { get; set; }

        [IgnoreDataMember]
        public TimeSpan? LifeSpan
        {
            get
            {
                if (BirthDate.HasValue)
                {
                    return DateTime.Now - BirthDate.Value;
                }

                return null;
            }
        }
    }
}
