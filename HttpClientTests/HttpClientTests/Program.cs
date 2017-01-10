using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientTests
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = 1000000;
            new ManyHttpClientInstantiations().Run(count);
            new ManyHttpClientLookups().Run(count);
            Console.ReadLine();
        }
    }
}
