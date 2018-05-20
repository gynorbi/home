using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionalConfiguration
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private IDictionary<string, string> configuration = new Dictionary<string, string>
        {
            {"key1", "value1" },
            {"key2", "value2" },
            {"key3", "value3" },
            {"key4", "value4" },
            {"key5", "value5" }
        };
        public IDictionary<string, string> GetConfiguration()
        {
            return configuration;
        }

        public IDictionary<string, string> GetConfigurationNewInstance()
        {
            return new Dictionary<string, string>(configuration);
        }
    }
}
