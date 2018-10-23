using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionDemos
{
    public class AnimalCollection : IEnumerable
    {
        private readonly Animal[] _animalCollection;

        public AnimalCollection(Animal[] animals)
        {
            _animalCollection = new Animal[animals.Length];
            for(int i = 0; i < animals.Length; i++)
            {
                _animalCollection[i] = animals[i];
            }
        }

        public IEnumerator GetEnumerator()
        {
            return _animalCollection.GetEnumerator();
        }
    }
}
