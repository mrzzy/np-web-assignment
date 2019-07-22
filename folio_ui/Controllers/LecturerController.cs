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
        public async Task<ActionResult> Details(int id)
        {
            
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000");
            HttpResponseMessage response = await
             client.GetAsync("/api/lecturer/" + id.ToString());
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                 Lecturer lecturerList =
                 JsonConvert.DeserializeObject<Lecturer>(data);
                return View(lecturerList);
            }
            else
            {
                return View(new Lecturer());
            }
        }

        // GET: Lecturer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Lecturer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Lecturer/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            // Make Web API call to get a list of Lecturers related to a BookId
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000");
            HttpResponseMessage response = await
             client.GetAsync("/api/lecturer/" + id.ToString());
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                Lecturer lecturerList =
                JsonConvert.DeserializeObject<Lecturer>(data);
                return View(lecturerList);
            }
            else
            {
                return View(new Lecturer());
            }
        }

        //POST: Lecturer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Lecturer lecturer)
        {
            //Transfer data read to a Lecturer object

            //Make Web API call to post the vote object
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000");
            HttpResponseMessage response = await
             client.PostAsJsonAsync("/api/lecturer/update/" + id.ToString(), lecturer);
            if (response.IsSuccessStatusCode)
            {
                
                return RedirectToAction("Details", id);
            }
            else
            {
                return RedirectToAction("Index", "Lecturer");
            }
        }

        // GET: Lecturer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Lecturer/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Lecturer/ViewMentees/5
        public async Task<ActionResult> ViewMentees(int id)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000");
            HttpResponseMessage response = await client.GetAsync("/api/lecturer/mentees/" + id.ToString());
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                List<Student> studentList = JsonConvert.DeserializeObject<List<Student>>(data);
                return View(studentList);
            }
            else
            {
                return View(new List<Student>());
            }
        }
    }
}
