using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using folio.Models;
using System.Net;

namespace folio_ui.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public async Task<ActionResult> Index()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000");
            HttpResponseMessage response = await client.GetAsync("/api/students/details");
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

        // GET: Student/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            // Read BookId and Justification from HTML Form
            string name = collection["item.Name"];
            string course = collection["item.Course"];
            string photo = collection["item.Photo"];
            string description = collection["item.Description"];
            string achievement = collection["item.Achievement"];
            string externallink = collection["item.ExternalLink"];

            // Transfer data read to a vote object
            Student student = new Student();
            student.Name = name;
            

            // Make Web API call to post the vote object
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://ictonejourney.com");
            HttpResponseMessage response = await
             client.PostAsJsonAsync("/api/votes", vote);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                // Successful – code 201 returned
                return RedirectToAction("Details", new { id = bookid });
            }
            else
            {
                // Unsuccessful – other returned code
                TempData["BookId"] = bookid;
                TempData["Justification"] = justification;
                TempData["Message"] = "Fail to add vote record!";
                return RedirectToAction("Index", "Book");
            }
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: Student/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Student/Delete/5
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
    }
}