using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FunctionalConfiguration.Controllers
{
    [Produces("application/json")]
    [Route("api/Configuration")]
    public class ConfigurationController : Controller
    {
        private IConfigurationProvider configurationProvider;
        public ConfigurationController(IConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
        }
        // GET: api/Configuration
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var dictionary = configurationProvider.GetConfiguration();
            return dictionary.Select(kvp =>$"{{{kvp.Key}:{kvp.Value}}}");
        }
    }
}
