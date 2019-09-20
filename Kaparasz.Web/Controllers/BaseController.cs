using System;
using System.Diagnostics;
using Kaparasz.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Kaparasz.Web.Controllers
{
    public abstract class BaseController: Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}