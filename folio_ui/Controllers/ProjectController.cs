using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public async Task<ActionResult> Create()
        {

            return View();

        }
        public async Task<ActionResult> Edit(int id)
        {
            // Make Web API call to get a list of Lecturers related to a BookId
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000");
            HttpResponseMessage response = await
             client.GetAsync("/api/project/" + id.ToString());
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                Project projectList =
                JsonConvert.DeserializeObject<Project>(data);
                return View(projectList);
            }
            else
            {
                return View(new Project());
            }
        }
        [HttpPost]
        public async Task<ActionResult> Edit(int id,  Project project , IFormCollection collection)
        {
            string Title = collection["Model.Title"];
            string ProjectUrl = collection["Model.ProjectUrl"];
            string ProjectPoster = collection["Model.ProjectPoster"];
            string Description = collection["Model.Description"];
            Project projects = new Project();
            projects.Title = Title;
            projects.ProjectUrl = ProjectUrl;
            projects.ProjectPoster = ProjectPoster;
            projects.Description = Description;


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000");
            HttpResponseMessage response = await
            
            client.PostAsJsonAsync("/api/project/update/" + id.ToString(), projects);
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("Detail", id);
            }
            else
            {
                return RedirectToAction("Index", "Project");
            }
        }
        public async Task<ActionResult> Detail(int id, Project project , [FromQuery] int student)
        {
            return View();
        }
    }
}
