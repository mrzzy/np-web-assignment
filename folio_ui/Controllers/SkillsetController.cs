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
using folio.Services.API;
using System.Text;

namespace folio_ui.Controllers
{
    public class SkillsetController : Controller
    {
        // GET: Skillset
        public async Task<ActionResult> Index(int id)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000");
            HttpResponseMessage response = await client.GetAsync("/api/skillset/details");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                List<SkillSet> skillSetList = JsonConvert.DeserializeObject<List<SkillSet>>(data);
                return View(skillSetList);
            }
            else
            {
                return View(new List<SkillSet>());
            }
        }

        public async Task<ActionResult> Details(int id)
        {
            // Make Web API call to get a list of votes related to a BookId
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000");
            HttpResponseMessage response = await
             client.GetAsync("/api/skillset/" + id.ToString());
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                List<SkillSet> skillSetList = JsonConvert.DeserializeObject<List<SkillSet>>(data);
                return View(skillSetList);
            }
            else
            {
                return View(new List<SkillSet>());
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SkillSet skillSet)
        {
            APIClient client = new APIClient(HttpContext);
            string skillSetJson = JsonConvert.SerializeObject(skillSet);

            APIResponse response = client.CallAPI("POST", "/api/skillset/create",
                new StringContent(skillSetJson, Encoding.UTF8, "application/json"));

            return View();
        }

        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000");
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/skillset/delete/" + id.ToString(), id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", id);
            }
            else
            {
                return RedirectToAction("Index", "Skillset");
            }
        }

        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, SkillSet skillSet)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000");
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/skillset/update/" + id.ToString(), skillSet);
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("Details", id);
            }
            else
            {
                return RedirectToAction("Index", "Skillset");
            }
        }
    }
}