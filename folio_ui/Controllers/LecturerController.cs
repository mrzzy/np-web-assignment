using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using folio_ui.Models;
using folio.Models;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.IO;
using folio.Services.API;

namespace folio_ui.Controllers
{
    public class LecturerController : Controller
    {
        // GET: Lecturers
        public async Task<ActionResult> Index()
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000");
            HttpResponseMessage response = await client.GetAsync("/api/lecturers/details");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                List<Lecturer> LecturerList = JsonConvert.DeserializeObject<List<Lecturer>>(data);
                return View(LecturerList);
            }
            else
            {
                return View(new List<Lecturer>());
            }
        }

        // GET: Lecturer/Details/5
        public ActionResult Details(int id)
        {
            APIClient api = new APIClient(HttpContext);
            APIResponse response = api.CallAPI("GET", "/api/lecturer/" + id);
            Lecturer lecturer = JsonConvert.DeserializeObject<Lecturer>(response.Content);

            return View(lecturer);
        }

        // GET: Lecturer/Create
        public ActionResult SignUp()
        {
            return View();
        }

        // POST: Lecturer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignUp(Lecturer lecturer)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000");
            HttpResponseMessage response = await
             client.PostAsJsonAsync("/api/lecturer/create", lecturer);
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("Login", "Auth");
            }
            else
            {
                return RedirectToAction("SignUp", "Lecturer");
            }
        }

        // GET: Lecturer/Edit/5
        public ActionResult Edit(int id)
        {
            APIClient api = new APIClient(HttpContext);
            APIResponse response = api.CallAPI("GET", "/api/lecturer/" + id);
            Lecturer lecturer = JsonConvert.DeserializeObject<Lecturer>(response.Content);

            return View(lecturer);
        }

        //POST: Lecturer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Lecturer lecturer)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000");
            HttpResponseMessage response = await
             client.PostAsJsonAsync("/api/lecturer/update/" + id.ToString(), lecturer);
            if (response.IsSuccessStatusCode)
            {
                
                return RedirectToAction("Details",new { id = id });
            }
            else
            {
                return RedirectToAction("Index", "Lecturer");
            }
        }

        // GET: Lecturer/UploadPhoto/5
        public async Task<ActionResult> UploadPhoto(int id)
        {
            // Make Web API call to get a list of Lecturers related to a BookId
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000");
            HttpResponseMessage response = await
             client.GetAsync("/api/lecturer/" + id.ToString());
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                LecturerViewModel lecturerList =
                JsonConvert.DeserializeObject<LecturerViewModel>(data);
                return View(lecturerList);
            }
            else
            {
                return View(new LecturerViewModel());
            }
        }

        //POST: Upload Photo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPhoto( int id,LecturerViewModel lvm)
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
                    string uploadedFile = lvm.Name + fileExt;
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
                    lvm.Photo = uploadedFile;
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

        //GET: Lecturer/ChangePassword/5
        public ActionResult ChangePassword(int id)
        {
            APIClient api = new APIClient(HttpContext);
            APIResponse response = api.CallAPI("GET", "/api/lecturer/" + id);
            Lecturer lecturer = JsonConvert.DeserializeObject<Lecturer>(response.Content);

            return View(lecturer);
        }

        // POST: Lecturer/ChangePassword/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(int id, Lecturer lecturer)
        {

            //Make Web API call to post the vote object
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000");
            HttpResponseMessage response = await
             client.PostAsJsonAsync("/api/lecturer/changePW/" + id.ToString(), lecturer);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {

                return RedirectToAction("ChangePassword");
            }
        }

        

        // POST: Lecturer/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            //Make Web API call to post the vote object
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000");
            HttpResponseMessage response = await
             client.PostAsJsonAsync("/api/lecturer/delete/" + id.ToString(), id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["Msg"] = "You are not Wasted! Another One";
                return RedirectToAction("Edit", new { id = id });
            }
        }

        // GET: Lecturer/ViewMentees/5
        public ActionResult ViewMentees(int id)
        {
            APIClient api = new APIClient(HttpContext);
            APIResponse response = api.CallAPI("GET", "/api/lecturer/mentees/" + id);
            List<Student> lecturer = JsonConvert.DeserializeObject<List<Student>>(response.Content);

            return View(lecturer);

            
        }

        
    }
}
