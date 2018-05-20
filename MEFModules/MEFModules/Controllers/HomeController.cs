using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MEFModules.Models;
using Common;
using System.Composition.Hosting;
using System.Composition.Convention;
using System.Composition;

namespace MEFModules.Controllers
{
    public class HomeController : Controller
    {
        private ModulesViewModel GetModules()
        {
            var path = @"c:\temp\MEFModules";
            var conventions = new ConventionBuilder();
            conventions
                .ForTypesDerivedFrom<IModule>()
                .Export<IModule>()
                .Shared();

            var configuration = new ContainerConfiguration()
                .WithAssembliesInPath(path, conventions);
            ModulesViewModel modules;
            using (var container = configuration.CreateContainer())
            {

                modules = new ModulesViewModel();
                container.SatisfyImports(modules);
            }
            return modules;
        }

        public IActionResult Index()
        {
            // Dupa cum vezi aici la fiecare refresh de pagina incarci lista de module.
            // Ideal ar fi sa existe o instanta de "manager" in memorie, creat la pornirea aplicatiei
            // "managerul" ar face watch pe folderul unde sunt dll-urile, si daca se adauga unul sau se sterge unul, 
            // incarca lista din nou. Pagina trebuie servita mereu cu date din memorie, altfel o sa fie lent.
            var modules = GetModules();
            return View(modules);
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
