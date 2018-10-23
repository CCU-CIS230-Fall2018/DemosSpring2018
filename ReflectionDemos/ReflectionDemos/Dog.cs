using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionDemos
{
    public class Dog : Animal
    {
        public Dog()
        {
        }

        public void Bark()
        {
            Console.WriteLine("Ruff! Ruff!");
        }

        private void Bark(int times, string barkSound)
        {
            for(int i = 0; i < times; i++)
            {
                Console.Write(barkSound);
            }
            Console.WriteLine();
        }

        private void Breathe()
        {
            Console.WriteLine("Inhale...Exhale");
        }
    }
}
