using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientTests
{
    public class ManyHttpClientInstantiations
    {
        public void Run(int count)
        {
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
            {
                using (var handler = new WebRequestHandler())
                {
                    using (var client = new HttpClient(handler))
                    {
                        // and doing nothing
                    }
                }
            }
            sw.Stop();
            Console.WriteLine("ManyHttpClientInstantiations - Elapsed time for instantiating {0} HttpClient classes:{1}", count, sw.ElapsedMilliseconds);
        }
    }
}
