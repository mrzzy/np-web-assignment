using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using folio.Services.API;
using folio_ui.Models;
using folio.Models;

namespace folio_ui.Controllers
{
    public class HomeController : Controller
    {
        // Landing page of the side
        public IActionResult Index()
        {
            return View();
        }
    
        // Server Error Page
        [ResponseCache(
                Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id 
                    ?? HttpContext.TraceIdentifier });
        }
    }
}
