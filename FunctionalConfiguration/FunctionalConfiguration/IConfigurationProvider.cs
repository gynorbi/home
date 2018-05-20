using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionalConfiguration
{
    public interface IConfigurationProvider
    {
        IDictionary<string,string> GetConfiguration();
        IDictionary<string,string> GetConfigurationNewInstance();
    }
}
