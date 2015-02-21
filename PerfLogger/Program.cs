using System;
using System.Diagnostics;
using System.Linq;

namespace PerfLogger
{
    class Program
    {
        static void Main(string[] args)
        {
            var sum = 0.0;
            using (PrefLogggger.Measure("100M for iterations"))
                for (var i = 0; i < 100000000; i++) sum += i;
            using (PrefLogggger.Measure("100M LINQ iterations"))
                sum -= Enumerable.Range(0, 100000000).Sum(i => (double)i);
            Console.WriteLine(sum);
        }
    }

    public class PrefLogggger : IDisposable
    {
        Stopwatch time;
        string data;

        void Start()
        {
            time = Stopwatch.StartNew();
        }

        void IDisposable.Dispose()
        {
            time.Stop();
            Console.WriteLine(data + time.ElapsedMilliseconds);
        }

        public static PrefLogggger Measure(string text)
        {
            PrefLogggger a = new PrefLogggger();
            a.data = text;
            a.Start();
            return a;
        }
    }
}