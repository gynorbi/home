using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonThings
{
    public static class SwTimer
    {
        public static long Time(Action doSomething)
        {
            var sw = Stopwatch.StartNew();
            doSomething();
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        public static async Task<long> Time(Func<Task> doSomething)
        {
            var sw = Stopwatch.StartNew();
            await doSomething();
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }
    }
}
