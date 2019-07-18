using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using folio.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace folio_ui.Controllers
{
    public class ProjectController : Controller
    {
        // GET: /<controller>/
        public async Task<ActionResult> Index()

        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000");
            HttpResponseMessage response = await client.GetAsync("/api/projects/details");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                List<Project> projectList = JsonConvert.DeserializeObject<List<Project>>(data);
                return View(projectList);
            }
            else
            {
                return View(new List<Project>());
            }
        }


    }
}
