using Common;
using System;
using System.Collections.Generic;
using System.Composition;

namespace ModuleBeta
{
    [Export(typeof(IModule))]
    [Shared]
    public class Module : IModule
    {
        public IEnumerable<string> Commands
        {
            get => throw new NotImplementedException();
        }
        public string Name
        {
            get => "ModuleBeta";
        }
    }
}
