using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Concurrency
{
    class Program
    {
        static object countLock = new object();

        static void Main(string[] args)
        {
            ConcurrentWrite();

            //ConcurrentThreadCount(100);

            //AsyncDownload();

            //RaceCondition();

            //Deadlock();

            //LazyInitialization();

            //TimerDemo();

            //PLinqDemo();

            //ParallelDemo();
        }

        static void ConcurrentWrite()
        {
            Stopwatch sw = new Stopwatch();

            Console.WriteLine();

            sw.Start();
            WriteValue("0");
            WriteValue("1");
            WriteValue("2");
            WriteValue("3");
            sw.Stop();

            //sw.Start();
            //Thread t0 = new Thread(() => WriteValue("0")); // { IsBackground = true };
            //Thread t1 = new Thread(() => WriteValue("1")); // { IsBackground = true };
            //Thread t2 = new Thread(() => WriteValue("2")); // { IsBackground = true };
            //Thread t3 = new Thread(() => WriteValue("3")); // { IsBackground = true };
            //t0.Start();
            //t1.Start();
            //t2.Start();
            //t3.Start();
            //t0.Join();
            //t1.Join();
            //t2.Join();
            //t3.Join();
            //sw.Stop();

            //sw.Start();
            //CountdownEvent countdown = new CountdownEvent(4);
            //ThreadPool.QueueUserWorkItem(s => { WriteValue("0"); countdown.Signal(); });
            //ThreadPool.QueueUserWorkItem(s => { WriteValue("1"); countdown.Signal(); });
            //ThreadPool.QueueUserWorkItem(s => { WriteValue("2"); countdown.Signal(); });
            //ThreadPool.QueueUserWorkItem(s => { WriteValue("3"); countdown.Signal(); });
            //countdown.Wait();
            //sw.Stop();

            //sw.Start();
            //Task.WaitAll(
            //    Task.Run(() => WriteValue("0")),
            //    Task.Run(() => WriteValue("1")),
            //    Task.Run(() => WriteValue("2")),
            //    Task.Run(() => WriteValue("3"))
            //    );
            //sw.Stop();

            //var task = Task.Run(() => 5 + 6);
            //Console.WriteLine(task.Result);

            Console.WriteLine("Elapsed Time: {0:n0}ms", sw.ElapsedMilliseconds);
        }

        static void WriteValue(string value, int iterations = 100)
        {
            for (int i = 0; i < iterations; i++)
            {
                // Simulate some work.
                Thread.Sleep(10);

                Console.WriteLine(value);
            }
        }

        static void ConcurrentThreadCount(long count)
        {
            long actualCount = 0;

            for (long i = 0; i < count; i++)
            {
                // Simulate some work before incrementing the count.
                Thread.Sleep(1);

                actualCount++;
            }

            //List<Thread> threads = new List<Thread>();

            //for (long i = 0; i < count; i++)
            //{
            //    Thread thread = new Thread(() =>
            //    {
            //        // Simulate some work before incrementing the count.
            //        Thread.Sleep(1);

            //        actualCount++;
            //    });

            //    thread.Start();

            //    threads.Add(thread);
            //}

            //foreach (var thread in threads)
            //{
            //    thread.Join();
            //}

            Console.WriteLine("Expected Count: {0:n0}", count);
            Console.WriteLine("Actual Count: {0:n0}", actualCount);
        }

        private static void AsyncDownload()
        {
            string[] urls = new string[]
            {
                "http://msdn.microsoft.com",
                "http://www.yahoo.com",
                "http://www.google.com"
            };

            List<Task> tasks = new List<Task>();

            foreach (var url in urls)
            {
                tasks.Add(GetAsync(url)
                    .ContinueWith(t => Console.WriteLine($"{url}: page size: {t.Result:n0}.")));
            }

            Task.WaitAll(tasks.ToArray());
        }

        private static async Task<int> GetAsync(string url)
        {
            // You need to add a reference to System.Net.Http to declare client.
            HttpClient client = new HttpClient();

            Console.WriteLine($"{url}: starting download...");

            // GetStringAsync returns a Task<string>. That means that when you await the
            // task you'll get a string (urlContents).
            Task<string> getStringTask = client.GetStringAsync(url);

            // You can do work here that doesn't rely on the string from GetStringAsync.
            Console.WriteLine($"{url}: doing some other work here...");

            // The await operator suspends AccessTheWebAsync.
            //  - AccessTheWebAsync can't continue until getStringTask is complete.
            //  - Meanwhile, control returns to the caller of AccessTheWebAsync.
            //  - Control resumes here when getStringTask is complete. 
            //  - The await operator then retrieves the string result from getStringTask.
            string urlContents = await getStringTask;

            Console.WriteLine($"{url}: download complete.");

            // The return statement specifies an integer result.
            // Any methods that are awaiting AccessTheWebAsync retrieve the length value.
            return urlContents.Length;
        }

        static void RaceCondition(long count = 100)
        {
            long actualCount = 0;

            for (long i = 0; i < count; i++)
            {
                // Simulate some work before incrementing the count.
                Thread.Sleep(1);

                actualCount++;
            }

            //List<Thread> threads = new List<Thread>();

            //for (long i = 0; i < count; i++)
            //{
            //    Thread thread = new Thread(() =>
            //    {
            //        // Simulate some work before incrementing the count.
            //        Thread.Sleep(1);

            //        actualCount++;
            //    });

            //    thread.Start();

            //    threads.Add(thread);
            //}

            //foreach (var thread in threads)
            //{
            //    thread.Join();
            //}

            Console.WriteLine("Expected Count: {0:n0}", count);
            Console.WriteLine("Actual Count: {0:n0}", actualCount);
        }

        static void Deadlock()
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

        static void LazyInitialization()
        {
            Lazy<Oven> oven = new Lazy<Oven>(() => new Oven());

            List<Task> tasks = new List<Task>();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Run(() => Console.WriteLine($"IsPreHeated? {oven.Value.IsPreHeated}")));
                Thread.Sleep(1000);
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"Oven instance count: {Oven.InstanceCount}");
        }

        static void TimerDemo()
        {
            using (Timer t = new Timer(s => Console.WriteLine("Tick"), null, 0, 1000))
            {
                Thread.Sleep(10000);
            }
        }

        static void PLinqDemo()
        {
            var source = Enumerable.Range(100, 2000);

            Stopwatch sw = new Stopwatch();

            sw.Start();

            // Result sequence might be out of order.
            var parallelQuery = from num in source.AsParallel()
                                where num % 10 == 0
                                select num;

            // Fluent/Method syntax is also supported
            //var parallelQuery = source.AsParallel().Where(n => n % 10 == 0).Select(n => n);
            //var parallelQuery = source.AsParallel().WithDegreeOfParallelism(2).Where(n => n % 10 == 0).Select(n => n);

            var result = parallelQuery.ToArray();

            sw.Stop();

            foreach (var n in parallelQuery)
            {
                Console.WriteLine(n);
            }

            Console.WriteLine("Elapsed time: {0:n0}ms", sw.ElapsedMilliseconds);
        }

        static void ParallelDemo()
        {
            var data = new ConcurrentBag<string>();

            Parallel.Invoke(
                () => data.Add(new WebClient().DownloadString("http://www.yahoo.com")),
                () => data.Add(new WebClient().DownloadString("http://www.google.com"))
                );

            Console.WriteLine(data.Count);
        }
    }
}
