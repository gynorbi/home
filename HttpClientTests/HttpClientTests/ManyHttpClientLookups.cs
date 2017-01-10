using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientTests
{
    public class ManyHttpClientLookups
    {
        public void Run(int count)
        {
            var dictionary = new Dictionary<string, HttpClient>();
            for (int i = 0; i < count; i++)
            {
                dictionary.Add(i.ToString(), new HttpClient());
            }
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
            {
                var client = dictionary[i.ToString()];
            }
            sw.Stop();
            Console.WriteLine("ManyHttpClientLookups - Elapsed time for looking up {0} HttpClient classes:{1}", count, sw.ElapsedMilliseconds);
        }
    }
}
