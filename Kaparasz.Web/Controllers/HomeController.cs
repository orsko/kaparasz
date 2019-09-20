using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Kaparasz.Web.Controllers
{
    public class HomeController : BaseController
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        // GET: /<controller>/Privacy
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
