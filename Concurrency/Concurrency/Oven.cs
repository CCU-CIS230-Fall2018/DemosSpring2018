using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Concurrency
{
    public sealed class Oven : IDisposable
    {
        static int instanceCount = 0;
        Timer preheatTimer;

        public Oven()
        {
            Interlocked.Increment(ref instanceCount);
            PreHeat();
        }

        public static int InstanceCount => instanceCount;

        public bool IsPreHeated { get; set; }

        public void Dispose()
        {
            preheatTimer.Dispose();
        }

        public void PreHeat()
        {
            preheatTimer = new Timer(s => IsPreHeated = true, null, 5000, Timeout.Infinite);
        }
    }
}
