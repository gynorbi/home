﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComposerGatewayInProc
{
    class Program
    {
        static void Main(string[] args)
        {
            var listener = new SimpleListener();
            listener.StartListeningOn("http://localhost:88/");
        }
    }
}
