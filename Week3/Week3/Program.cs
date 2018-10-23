using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3
{
    class Program
    {
        static void Main(string[] args)
        {
            ExtensionMethods();
            //QueueSample();
            //StackSample();
            //GenericsSamples();
        }

        private static void ExtensionMethods()
        {
            Animal fred = new Animal();
            fred.Butcher();

            List<Animal> animals = new List<Animal>
            {
                new Zebra(),
                new Lion { Color = "blue" },
                new Animal()
            };

            var blueAnimals = animals.Where(a => a.Color == "blue");
            foreach(var a in blueAnimals)
            {
                Console.WriteLine(a.GetType().FullName);
            }
        }

        static void QueueSample()
        {
            Queue<string> strings = new Queue<string>();
            Console.WriteLine(strings.Count);
            strings.Enqueue("abc");
            strings.Enqueue("def");
            strings.Enqueue("ghi");
            Console.WriteLine(strings.Count);
            foreach (var s in strings)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine(strings.Count);
            int count = strings.Count;
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine(strings.Dequeue());
            }
            Console.WriteLine(strings.Count);
        }

        static void StackSample()
        {
            Stack<string> strings = new Stack<string>();
            Console.WriteLine(strings.Count);
            strings.Push("abc");
            strings.Push("def");
            strings.Push("ghi");
            Console.WriteLine(strings.Count);
            foreach (var s in strings)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine(strings.Count);
            int count = strings.Count;
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine(strings.Pop());
            }
            Console.WriteLine(strings.Count);
        }

        static void  GenericsSamples()
        {
            Dictionary<int, Animal> animals = new Dictionary<int, Animal>();
            animals.Add(42, new Zebra { Color = "Blue" });

            foreach (var animal in animals)
            {
                Console.WriteLine($"{animal.Key}: {animal.Value.Color}");
            }

            Console.WriteLine(animals[42].Color);
        }

        static void CastingSamples()
        {
            int x = 23;
            object y = x;
            x = (int)y;

            Console.WriteLine($"x:{x}, y:{y}");

            Lion l = new Lion { Color = "Yellow" };
            Zebra z = new Zebra() { Color = "White" };
            Animal a = z;
            Console.WriteLine(((Zebra)a).Color);
            //Fails at runtime: Console.WriteLine(((Lion)a).Color);

            Console.WriteLine(a.GetType().FullName);

            Animal a2 = new Animal();
            Zebra z2 = (Zebra)a2;
        }
    }
}
