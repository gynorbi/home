using Common;
using System;
using System.Collections.Generic;
using System.Composition;

namespace ModuleAlpha
{
    [Export(typeof(IModule))]
    [Shared]
    public class Module : IModule
    {
        public IEnumerable<string> Commands => throw new NotImplementedException();

        public string Name => "ModuleAlpha";
    }
}
