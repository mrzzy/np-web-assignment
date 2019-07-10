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
            client.BaseAddress = new Uri("https://localhost:5000");
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
            // Make Web API call to get a list of votes related to a BookId
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:5000");
            HttpResponseMessage response = await
             client.GetAsync("/api/lecturer/" + id.ToString());
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                List<Lecturer> lecturerList =
                 JsonConvert.DeserializeObject<List<Lecturer>>(data);
                return View(lecturerList);
            }
            else
            {
                return View(new List<Lecturer>());
            }
        }

        // GET: Vote/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vote/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
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

        // GET: Vote/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Vote/Edit/5
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

        // GET: Vote/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Vote/Delete/5
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
