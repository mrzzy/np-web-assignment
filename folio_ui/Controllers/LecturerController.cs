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
using System.Text;

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
        public ActionResult SignUp(Lecturer lecturer)
        {
            var content = JsonConvert.SerializeObject(lecturer);
            string contentType = "application/json";
            var sContent = new StringContent(content, Encoding.UTF8, contentType);

            APIClient api = new APIClient(HttpContext);
            APIResponse response = api.CallAPI("POST", "/api/lecturer/create", sContent);

            return RedirectToAction("Login", "Auth");

            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost:5000");
            //HttpResponseMessage response = await
            // client.PostAsJsonAsync("/api/lecturer/create", lecturer);
            //if (response.IsSuccessStatusCode)
            //{

            //    return RedirectToAction("Login", "Auth");
            //}
            //else
            //{
            //    return RedirectToAction("SignUp", "Lecturer");
            //}
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
        public ActionResult Edit(int id, Lecturer lecturer)
        {
            var content = JsonConvert.SerializeObject(lecturer);
            string contentType = "application/json";
            
            var sContent = new StringContent(content,Encoding.UTF8,contentType);

            APIClient api = new APIClient(HttpContext);

            APIResponse response = api.CallAPI("POST", "/api/lecturer/update/" + id, sContent);

            return RedirectToAction("Details", new { id = id });
        }

        // GET: Lecturer/UploadPhoto/5
        public ActionResult UploadPhoto(int id)
        {

            APIClient api = new APIClient(HttpContext);
            APIResponse response = api.CallAPI("GET", "/api/lecturer/" + id);
            LecturerViewModel lecturer = JsonConvert.DeserializeObject<LecturerViewModel>(response.Content);

            return View(lecturer);

            
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
        public ActionResult ChangePassword(int id, Lecturer lecturer)
        {
            var content = JsonConvert.SerializeObject(lecturer);
            string contentType = "application/json";
            var sContent = new StringContent(content, Encoding.UTF8, contentType);

            APIClient api = new APIClient(HttpContext);
            APIResponse response = api.CallAPI("POST", "/api/lecturer/changePW/" + id, sContent);

            return RedirectToAction("Index");

            ////Make Web API call to post the vote object
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost:5000");
            //HttpResponseMessage response = await
            // client.PostAsJsonAsync("/api/lecturer/changePW/" + id.ToString(), lecturer);
            //if (response.IsSuccessStatusCode)
            //{
            //    return RedirectToAction("Index");
            //}
            //else
            //{

            //    return RedirectToAction("ChangePassword");
            //}
        }

        

        // POST: Lecturer/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            APIClient api = new APIClient(HttpContext);
            APIResponse response = api.CallAPI("POST", "/api/lecturer/delete/" + id);

            return RedirectToAction("Index", "Home");

           
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
