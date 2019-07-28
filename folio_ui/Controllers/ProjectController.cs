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
using System.IO;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace folio_ui.Controllers
{
    public class ProjectController : Controller 
    {
        // GET: /<controller>/
        public async Task<ActionResult> Index(int id)

        {
            APIClient api = new APIClient();

            APIResponse response = api.CallAPI("GET", "/api/project/" + id);
            Project project = JsonConvert.DeserializeObject<Project>(response.Content);

            response = api.CallAPI("GET", "/api/projects?student=" + id);
            List<int> studentIds = JsonConvert.DeserializeObject<List<int>>(response.Content);
            IEnumerable<Project> students = studentIds.Select((studentId) => {
                response = api.CallAPI("GET", "/api/project/" + studentId);
                return JsonConvert.DeserializeObject<Project>(response.Content);
            });
            ViewData["Projects"] = students;

            return View(project);
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
        // GET: Lecturer/UploadPhoto/5
        public async Task<ActionResult> UploadPoster(int id)
        {
            // Make Web API call to get a list of Lecturers related to a BookId
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000");
            HttpResponseMessage response = await
             client.GetAsync("/api/project/" + id.ToString());
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                ProjectViewModel projectList =
                JsonConvert.DeserializeObject<ProjectViewModel>(data);
                return View(projectList);
            }
            else
            {
                return View(new ProjectViewModel());
            }
        }

        //POST: Upload Photo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPoster(int id, ProjectViewModel lvm)
        {
            if (lvm.FileToUpload != null &&
            lvm.FileToUpload.Length > 0)
            {
                try
                {
                    // Find the filename extension of the file to be uploaded.
                    string fileExt = Path.GetExtension(
                     lvm.FileToUpload.FileName);
                    // Rename the uploaded file with the staff’s name.
                    string uploadedFile = lvm.ProjectPoster + fileExt;
                    // Get the complete path to the images folder in server
                    string savePath = Path.Combine(
                     Directory.GetCurrentDirectory(),
                     "wwwroot\\img", uploadedFile);
                    // Upload the file to server
                    using (var fileSteam = new FileStream(
                     savePath, FileMode.Create))
                    {
                        await lvm.FileToUpload.CopyToAsync(fileSteam);
                    }
                    lvm.ProjectPoster = uploadedFile;
                    ViewData["Message"] = "File uploaded successfully.";
                }
                catch (IOException)
                {
                    //File IO error, could be due to access rights denied
                    ViewData["Message"] = "File uploading fail!";
                }
                catch (Exception ex) //Other type of error
                {
                    ViewData["Message"] = ex.Message;
                }
            }
            return View(lvm);

        }
    }
}
   
