using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TcpClient;

namespace HttpListenerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleListenerExample("http://localhost:85/index/");
        }

        // This example requires the System and System.Net namespaces.
        public static void SimpleListenerExample(params string[] prefixes)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            // URI prefixes are required,
            // for example "http://contoso.com:8080/index/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // Create a listener.
            HttpListener listener = new HttpListener();
            // Add the prefixes.
            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }
            listener.Start();
            var rand = new Random(100);
            while (true)
            {
                var randomText = string.Join(string.Empty, Enumerable.Repeat('a', 5).Select(c => (char)('a' + ('z' % rand.Next('a','z')))));
                Console.WriteLine("Listening...");
                // Note: The GetContext method blocks while waiting for a request. 
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;

                //Dispatch request
                var responseText = TcpClientStuff.Connect("127.0.0.1", randomText);
                // Obtain a response object.
                HttpListenerResponse response = context.Response;
                // Construct a response.
                string responseString = string.Format(@"<HTML><BODY> Hello world {0} time(s)!</BODY></HTML>", responseText);
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                // You must close the output stream.
                output.Close();
            }
            listener.Stop();
        }
    }
}
