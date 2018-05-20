using System;
using System.Collections.Generic;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace MEFModules
{
    public static class ContainerConfigurationExtensions
    {
        public static ContainerConfiguration WithAssembliesInPath(this ContainerConfiguration configuration, string path, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return WithAssembliesInPath(configuration, path, null, searchOption);
        }

        public static ContainerConfiguration WithAssembliesInPath(this ContainerConfiguration configuration, string path, AttributedModelProvider conventions, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var files = Directory
                .GetFiles(path, "*.dll", searchOption);
           // Problema aici e ca le incarci din calea unde sunt si dupa aceea au lock pe ele.
           // Nu le mai poti suprascrie
           // Ideal ar fi sa existe o copiere automata intr-un shadow folder si sa le incarce de acolo. 
           // In ASP.NET exista asa ceva pentru fisierele din /bin asa ca presupun ca exista solutie si in ASP.NET Core
            var assemblies = files
                .Select(Assembly.LoadFile)
                .ToList();

            configuration = configuration.WithAssemblies(assemblies, conventions);

            return configuration;
        }
    }
}
