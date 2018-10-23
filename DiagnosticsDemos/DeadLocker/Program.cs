using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeadLocker
{
    class Program
    {
        static void Main(string[] args)
        {
            object lock1 = new object();
            object lock2 = new object();

            new Thread(() =>
            {
                lock (lock1)
                {
                    Console.WriteLine("{0}: Lock 1 acquired", Thread.CurrentThread.ManagedThreadId);

                    Thread.Sleep(1000);

                    // Deadlock will happen here.
                    lock (lock2)
                    {
                        Console.WriteLine("{0}: Lock 2 acquired", Thread.CurrentThread.ManagedThreadId);
                    };
                }
            }).Start();

            lock (lock2)
            {
                Console.WriteLine("{0}: Lock 2 acquired", Thread.CurrentThread.ManagedThreadId);

                Thread.Sleep(1000);

                // Deadlock will happen here.
                lock (lock1)
                {
                    Console.WriteLine("{0}: Lock 1 acquired", Thread.CurrentThread.ManagedThreadId);
                };
            }
        }
    }
}
