using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using folio.Models;
using folio.Services.API;

namespace folio_ui.Controllers
{
    public class SuggestionController : Controller
    {
        // GET: Suggestions
        public async Task<ActionResult> Index(int id)
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

        // GET: Suggestion/Details/5
        public async Task<ActionResult> Details(int id)
        {
            // Make Web API call to get a list of votes related to a SuggestionId
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000");
            HttpResponseMessage response = await
             client.GetAsync("/api/suggestion/student/" + id.ToString());
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                List<Suggestion> suggestionList =
                 JsonConvert.DeserializeObject<List<Suggestion>>(data);

                ViewData["studentId"] = id;
                return View(suggestionList);
            }
            else
            {
                return View(new List<Suggestion>());
            }
        }



        // GET: Suggestion/Create
        public ActionResult Create(int id)
        {
            ViewData["studentId"] = id;
            return View();
        }

        // POST: Suggestion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Suggestion suggestion)
        {
            
            //Make Web API call to post the vote object
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000");
            HttpResponseMessage response = await
             client.PostAsJsonAsync("/api/suggestion/create", suggestion);
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("Index","Lecturer");
            }
            else
            {
                return RedirectToAction("Create");
            }
        }

        // GET: Suggestion/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Suggestion/Edit/5
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

        // GET: Suggestion/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Suggestion/Delete/5
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

