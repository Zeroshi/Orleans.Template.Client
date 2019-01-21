using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Handler.Person;
using Microsoft.AspNetCore.Mvc;
using Orleans.Template.Client.Models;

namespace Orleans.Template.Client.Controllers
{
    public class HomeController : Controller
    {
        public readonly IPersonService _personService;

        public HomeController(IPersonService personService)
        {
            _personService = personService;
        }

        public IActionResult Index()
        {
            var result = _personService.SayHello("test");
            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
