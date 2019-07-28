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
        public ActionResult ViewProjMember(int id)
        {
            APIClient api = new APIClient(HttpContext);
            APIResponse response = api.CallAPI("GET", "/api/project/member/" + id);
            List<ProjectMember> projectMemberList = JsonConvert.DeserializeObject<List<ProjectMember>>(response.Content);

            return View(projectMemberList);


        }
        [HttpPost]
        public async Task<ActionResult> ViewProjMember(int id, ProjectMember projMember, int student)
        {

            APIClient client = new APIClient(HttpContext);
            string projectJson = JsonConvert.SerializeObject(projMember);
            APIResponse response = client.CallAPI("POST", "/api/project/assign/" + id + "?student=" + student);
                new StringContent(projectJson, Encoding.UTF8, "application/json");

            return View(projMember);
        
        }
      
        public async Task<ActionResult> UploadPoster(int id)
        {
            
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPoster(int id, ProjectViewModel pvm)
        {
            if (pvm.FileToUpload != null &&
            pvm.FileToUpload.Length > 0)
            {
                try
                {
                    // Find the filename extension of the file to be uploaded.
                    string fileExt = Path.GetExtension(
                     pvm.FileToUpload.FileName);
                    // Rename the uploaded file with the Project Poster’s name.
                    string uploadedFile = pvm.ProjectPoster + fileExt;
                    // Get the complete path to the images folder in server
                    string savePath = Path.Combine(
                     Directory.GetCurrentDirectory(),
                     "wwwroot\\img", uploadedFile);
                    // Upload the file to server
                    using (var fileSteam = new FileStream(
                     savePath, FileMode.Create))
                    {
                        await pvm.FileToUpload.CopyToAsync(fileSteam);
                    }
                    pvm.ProjectPoster = uploadedFile;
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
            return View(pvm);

        }
    }
}
   
