using Common;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace MEFModules.Models
{
    public class ModulesViewModel
    {
        [ImportMany]
        public IEnumerable<IModule> Modules { get; set; }
    }
}
