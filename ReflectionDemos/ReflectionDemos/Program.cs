using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionDemos
{
    class Program
    {
        static void Main(string[] args)
        {
            TypeInformationDemo();

            //TypeInstantiationDemo();

            //MemberInvocationDemo();

            //DynamicMethodDemo();

            //CodeDomDemo();

            //ExamPrep();
        }

        private static void TypeInformationDemo()
        {
            WriteTypeInformation(typeof(string));

            //WriteTypeInformation(typeof(Animal));

            Dog d = new Dog();
            //WriteTypeInformation(d.GetType());
        }

        private static void WriteTypeInformation(Type type)
        {
            Console.WriteLine($"AssemblyQualifiedName: {type.AssemblyQualifiedName}");
            Console.WriteLine($"BaseType: {type.BaseType}");
            Console.WriteLine($"IsAbstract: {type.IsAbstract}");
            Console.WriteLine($"IsClass: {type.IsClass}");
            Console.WriteLine($"IsEnum: {type.IsEnum}");
            Console.WriteLine($"IsGenericType: {type.IsGenericType}");
            Console.WriteLine($"IsPrimitive: {type.IsPrimitive}");

            //foreach (var property in type.GetProperties())
            //{
            //    Console.WriteLine($"Property: {property.Name}: {property.PropertyType.Name}");
            //    Console.WriteLine($"          CanRead: {property.CanRead}");
            //    Console.WriteLine($"          CanWrite: {property.CanWrite}");
            //}

            //foreach (var method in type.GetMethods())
            //{
            //    Console.WriteLine($"Method: {method.Name}: {method.ReturnType.Name}");
            //    Console.WriteLine($"        IsAbstract: {method.IsAbstract}");
            //    Console.WriteLine($"        IsPublic: {method.IsPublic}");
            //}
        }

        private static void TypeInstantiationDemo()
        {
            Dog d = Activator.CreateInstance(typeof(Dog)) as Dog;
            d.Bark();

            Dog d2 = typeof(Dog).GetConstructor(new Type[] { }).Invoke(null) as Dog;
            d2.Bark();
        }

        private static void MemberInvocationDemo()
        {
            Dog d = new Dog();

            foreach(var method in d.GetType().GetMethods())
                Console.WriteLine(method.Name);

            MethodInfo barkMethod = d.GetType().GetMethod("Bark", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(int), typeof(string) }, null);
            barkMethod.Invoke(d, new object[] { 9, "Yip!" });

            // Won't compile: d.Breathe();
            MethodInfo breatheMethod = d.GetType().GetMethod("Breathe", BindingFlags.NonPublic | BindingFlags.Instance);
            breatheMethod.Invoke(d, null);
        }

        private static void DynamicMethodDemo()
        {
            var method = new DynamicMethod("Hello", null, null);
            var generator = method.GetILGenerator();
            generator.EmitWriteLine("Hello world!");
            generator.Emit(OpCodes.Ret);
            method.Invoke(null, null);
        }

        private static void CodeDomDemo()
        {
            var compileUnit = new CodeCompileUnit();
            var hello = new CodeNamespace("Hello");
            hello.Imports.Add(new CodeNamespaceImport("System"));
            compileUnit.Namespaces.Add(hello);
            var helloClass = new CodeTypeDeclaration("HelloClass");
            hello.Types.Add(helloClass);

            var mainMethod = new CodeEntryPointMethod();
            CodeMethodInvokeExpression cs1 = new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression("System.Console"),
                "WriteLine", new CodePrimitiveExpression("Hello World!"));
            mainMethod.Statements.Add(cs1);
            helloClass.Members.Add(mainMethod);

            string sourceFile = GenerateCSharpCode(compileUnit);

            CompileCSharpCode(sourceFile, "HelloWorld.exe");
        }

        public static string GenerateCSharpCode(CodeCompileUnit compileunit)
        {
            // Generate the code with the C# code provider.
            CSharpCodeProvider provider = new CSharpCodeProvider();

            // Build the output file name.
            string sourceFile;
            if (provider.FileExtension[0] == '.')
            {
                sourceFile = "HelloWorld" + provider.FileExtension;
            }
            else
            {
                sourceFile = "HelloWorld." + provider.FileExtension;
            }

            // Create a TextWriter to a StreamWriter to the output file.
            using (StreamWriter sw = new StreamWriter(sourceFile, false))
            {
                IndentedTextWriter tw = new IndentedTextWriter(sw, "    ");

                // Generate source code using the code provider.
                provider.GenerateCodeFromCompileUnit(compileunit, tw,
                    new CodeGeneratorOptions());

                // Close the output file.
                tw.Close();
            }

            return sourceFile;
        }

        public static bool CompileCSharpCode(string sourceFile, string exeFile)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();

            // Build the parameters for source compilation.
            CompilerParameters cp = new CompilerParameters();

            // Add an assembly reference.
            cp.ReferencedAssemblies.Add("System.dll");

            // Generate an executable instead of
            // a class library.
            cp.GenerateExecutable = true;

            // Set the assembly file name to generate.
            cp.OutputAssembly = exeFile;

            // Save the assembly as a physical file.
            cp.GenerateInMemory = false;

            // Invoke compilation.
            CompilerResults cr = provider.CompileAssemblyFromFile(cp, sourceFile);

            if (cr.Errors.Count > 0)
            {
                // Display compilation errors.
                Console.WriteLine("Errors building {0} into {1}",
                    sourceFile, cr.PathToAssembly);
                foreach (CompilerError ce in cr.Errors)
                {
                    Console.WriteLine("  {0}", ce.ToString());
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Source {0} built into {1} successfully.",
                    sourceFile, cr.PathToAssembly);
            }

            // Return the results of compilation.
            if (cr.Errors.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static void ExamPrep()
        {
            AnimalCollection animals = new AnimalCollection(new Animal[] { new Dog { Name = "Fido" }, new Dog { Name = "Max" } });

            foreach(Animal animal in animals)
                Console.WriteLine(animal.Name);
        }
    }
}
