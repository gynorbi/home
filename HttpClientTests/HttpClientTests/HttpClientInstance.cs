using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpClientTests
{
    public class HttpClientInstance
    {
        private static int counter;
        private static int exceptioncounter;
        public async Task Call()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    Interlocked.Increment(ref counter);
                    Console.WriteLine("Running tasks: {0} - Exceptions: {1}", counter, exceptioncounter);
                    var response = await client.GetAsync("http://www.google.com");
                    Interlocked.Decrement(ref counter);
                    Console.WriteLine("Running tasks: {0} - Exceptions: {1}", counter, exceptioncounter);

                }
            }
            catch(Exception ex) {
                Interlocked.Increment(ref exceptioncounter);
                Console.WriteLine(ex.Message+"|"+ex.InnerException?.Message);
            }
        }
        public void Run(int count)
        {
            Console.WriteLine("Start of HttpClientInstance");
            var tasks = new List<Task>();
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
            {
                var task = Task.Run(Call);
                tasks.Add(task);
            }
            Task.WhenAll(tasks).Wait();
            //Parallel.For(0, count, new ParallelOptions { MaxDegreeOfParallelism = count },  async (i) =>
            //{
            //    using (var client = new HttpClient())
            //    {
            //        Console.WriteLine("Parallel index: {0} - Thread id: {1}", i, Thread.CurrentThread.ManagedThreadId);
            //        var response = await client.GetAsync("http://www.google.com");
            //    }
            //});
            sw.Stop();
            Console.WriteLine("HttpClientInstance - {0} ms", sw.ElapsedMilliseconds);
        }
    }
}
