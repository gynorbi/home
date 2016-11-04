using CGApplication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ComposerGatewayNamedPipes
{
    // This example requires the System and System.Net namespaces.
    public class SimpleListener
    {
        Application app;
        Process pipeClient;
        AnonymousPipeServerStream pipeServer;
        public SimpleListener()
        {
            pipeClient = new Process();

            pipeClient.StartInfo.FileName = "CGApplication.dll";

            pipeServer = new AnonymousPipeServerStream(
                PipeDirection.Out,
               HandleInheritability.Inheritable);
            
                // Pass the client process a handle to the server.
                pipeClient.StartInfo.Arguments =
                    pipeServer.GetClientHandleAsString();
                pipeClient.StartInfo.UseShellExecute = false;
                pipeClient.Start();

                pipeServer.DisposeLocalCopyOfClientHandle();

                try
                {
                    // Read user input and send that to the client process.
                    using (StreamWriter sw = new StreamWriter(pipeServer))
                    {
                        sw.AutoFlush = true;
                        // Send a 'sync message' and wait for client to receive it.
                        sw.WriteLine("SYNC");
                        pipeServer.WaitForPipeDrain();
                    }
                }
                // Catch the IOException that is raised if the pipe is broken
                // or disconnected.
                catch (IOException e)
                {
                    Console.WriteLine("[SERVER] Error: {0}", e.Message);
                }
            

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
                var serializedRequest = new System.
                pipeServer.Write(context.Request)            
                    }
            pipeClient.WaitForExit();
            pipeClient.Close();
            Console.WriteLine("[SERVER] Client quit. Server terminating.");
            pipeServer.Dispose();
            listener.Stop();
        }
    }
}
