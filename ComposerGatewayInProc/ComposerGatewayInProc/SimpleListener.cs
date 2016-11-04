using CGApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ComposerGatewayInProc
{
    // This example requires the System and System.Net namespaces.
    public class SimpleListener
    {
        Application app;
        public SimpleListener()
        {
            app = new Application();
        }

        public void StartListeningOn(params string[] prefixes)
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
            while (true)
            {
                Console.WriteLine("Listening...");
                // Note: The GetContext method blocks while waiting for a request. 
                var context = listener.GetContext();
                var request = context.Request;
                var response = context.Response;
                app.ProcessRequest(request, response);
            }
            listener.Stop();
        }
    }
}
