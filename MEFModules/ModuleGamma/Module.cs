using Common;
using System;
using System.Collections.Generic;
using System.Composition;

namespace ModuleGamma
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
            get => "ModuleGamme";
        }
    }
}
