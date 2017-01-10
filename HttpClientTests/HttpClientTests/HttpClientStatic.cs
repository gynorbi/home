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
    public class HttpClientStatic
    {
        private static int counter;
        private static int exceptioncounter;
        private static HttpClient client = new HttpClient();
        public async Task Call()
        {
            try
            {
                Interlocked.Increment(ref counter);
                Console.WriteLine("Running tasks: {0} - Exceptions: {1}", counter, exceptioncounter);
                var response = await client.GetAsync("http://www.google.com");
                Interlocked.Decrement(ref counter);
                Console.WriteLine("Running tasks: {0} - Exceptions: {1}", counter, exceptioncounter);
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref exceptioncounter);
                Console.WriteLine(ex.Message + "|" + ex.InnerException?.Message);
            }
        }
        public void Run(int count)
        {
            Console.WriteLine("Start of HttpClientStatic");
            var tasks = new List<Task>();
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
            {
                var task = Task.Run(Call);
                tasks.Add(task);
            }
            Task.WhenAll(tasks).Wait();
            
            sw.Stop();
            Console.WriteLine("HttpClientStatic - {0} ms", sw.ElapsedMilliseconds);
        }
    }
}
