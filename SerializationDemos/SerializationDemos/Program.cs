using KellermanSoftware.CompareNetObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Xml;

namespace SerializationDemos
{
    class Program
    {
        static void Main(string[] args)
        {
            AnimalList<Animal> list = new AnimalList<Animal>();
            list.Add(new Cat("Fluffy") { NumberOfLegs = 4 });
            list.Add(new Dog("Ruffles") { NumberOfLegs = 3, BirthDate = DateTime.Now - TimeSpan.FromDays(942) });

            //BinarySerialization(list);

            //DataContractSerialization(list);

            //DataContractJsonSerialization(list);

            //NewtonsoftJsonSerialization(list);
        }

        static void BinarySerialization<T>(AnimalList<T> list) where T : Animal
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream stream = File.Create("_b-animals.txt"))
            {
                formatter.Serialize(stream, list);
            }

            AnimalList<T> deserializedList = null;
            using (FileStream reader = File.OpenRead("_b-animals.txt"))
            {
                deserializedList = formatter.Deserialize(reader) as AnimalList<T>;
            }

            CompareObjects(list, deserializedList);
        }

        static void DataContractSerialization<T>(AnimalList<T> list) where T : Animal
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(AnimalList<T>));

            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true };
            using (XmlWriter writer = XmlWriter.Create("_dc-animals.xml", settings))
            {
                serializer.WriteObject(writer, list);
            }

            AnimalList<T> deserializedList = null;
            using (XmlReader reader = XmlReader.Create("_dc-animals.xml"))
            {
                deserializedList = serializer.ReadObject(reader) as AnimalList<T>;
            }

            CompareObjects(list, deserializedList);
        }

        static void DataContractJsonSerialization<T>(AnimalList<T> list) where T : Animal
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AnimalList<T>));

            using (FileStream fileStream = File.Create("_dcj-animals.json"))
            {
                serializer.WriteObject(fileStream, list);
            }

            AnimalList<T> deserializedList = null;
            using (FileStream reader = File.OpenRead("_dcj-animals.json"))
            {
                deserializedList = serializer.ReadObject(reader) as AnimalList<T>;
            }

            ComparisonConfig compareConfig = new ComparisonConfig
            {
                MaxMillisecondsDateDifference = 5,
                MembersToIgnore = new List<string> { "LifeSpan" }
            };

            CompareObjects(list, deserializedList, compareConfig);
        }

        static void NewtonsoftJsonSerialization<T>(AnimalList<T> list) where T : Animal
        {
            JsonSerializer serializer = new JsonSerializer
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Newtonsoft.Json.Formatting.Indented
            };

            using (StreamWriter writer = File.CreateText("_nsj-animals.json"))
            {
                serializer.Serialize(writer, list);
            }

            AnimalList<T> deserializedList = null;
            using (StreamReader reader = File.OpenText("_nsj-animals.json"))
            {
                deserializedList = serializer.Deserialize(reader, typeof(AnimalList<T>)) as AnimalList<T>;
            }

            CompareObjects(list, deserializedList);
        }

        static void CompareObjects(object firstObject, object secondObject, ComparisonConfig config = null, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            CompareLogic comparer = new CompareLogic();
            if (config != null)
            {
                comparer.Config = config;
            }

            var compareResult = comparer.Compare(firstObject, secondObject);

            if (compareResult.AreEqual)
            {
                Console.WriteLine();
                Console.WriteLine("{0}: Objects match", memberName);
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("{0}: Objects DO NOT match!", memberName);
                Console.WriteLine(compareResult.DifferencesString);
            }
        }
    }
}
