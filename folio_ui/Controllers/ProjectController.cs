using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using folio.Models;
using folio.Services.API;
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

        [HttpPost]
        public async Task<ActionResult> Create(int id, Project project, IFormCollection collection)
        {
            APIClient client = new APIClient(HttpContext);
            string projectJson = JsonConvert.SerializeObject(project);

            APIResponse response = client.CallAPI("POST", "/api/project/create",
                new StringContent(projectJson, Encoding.UTF8, "application/json"));
            Dictionary<string, int> reciept = JsonConvert.DeserializeObject<Dictionary<string, int>>(response.Content);
            int projectId = reciept["projectId"];
            
            if (response.StatusCode == 200)
            {
                // assign creator to project
                UserInfo creator = HttpContext.Items["UserInfo"] as UserInfo;
                response = client.CallAPI("POST", "/api/project/assign/" + projectId + "?student=" + creator.Id);
                Console.WriteLine(">>>>>>>" + response.StatusCode);
                
            }
            return View(project);
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
            //string Title = collection["Model.Title"];
            //string ProjectUrl = collection["Model.ProjectUrl"];
            //string ProjectPoster = collection["Model.ProjectPoster"];
            //string Description = collection["Model.Description"];
            //Project projects = new Project();
            //projects.Title = Title;
            //projects.ProjectUrl = ProjectUrl;
            //projects.ProjectPoster = ProjectPoster;
            //projects.Description = Description;


            APIClient client = new APIClient(HttpContext);
            string projectJson = JsonConvert.SerializeObject(project);

            APIResponse response = client.CallAPI("POST", "/api/project/update/" + id,
                new StringContent(projectJson, Encoding.UTF8, "application/json"));
            Dictionary<string, int> reciept = JsonConvert.DeserializeObject<Dictionary<string, int>>(response.Content);
      

         
            return View(project);
        }
        public async Task<ActionResult> Detail(int id, Project project , [FromQuery] int student)
        {
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
        [HttpGet]
        public async Task<ActionResult> ViewProjMember(int id)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000");
            HttpResponseMessage response = await client.GetAsync("/api/project/member/" + id.ToString());
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                List<ProjectMember> projectMemberList = JsonConvert.DeserializeObject<List<ProjectMember>>(data);
                return View(projectMemberList);
            }
            else
            {
                return View(new List<ProjectMember>());
            }
        }
        [HttpPost]
        public async Task<ActionResult> ViewProjMember(int id, ProjectMember projMember)
        {

            APIClient client = new APIClient(HttpContext);
            string projectJson = JsonConvert.SerializeObject(projMember);
            APIResponse response = client.CallAPI("POST", "/api/project/assign/" + id.ToString() + "?student=" + projectJson);
                new StringContent(projectJson, Encoding.UTF8, "application/json");

            return View(projMember);
        
        }  

    }
}
