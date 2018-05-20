using System;
using System.Collections.Generic;

namespace Common
{
    public interface IModule
    {
        IEnumerable<string> Commands { get; }
        string Name { get; }
    }
}
