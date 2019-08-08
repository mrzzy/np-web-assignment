using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using folio.Models;
using folio.Services.API;
using System.Text;

namespace folio_ui.Controllers
{
    public class SkillsetController : Controller
    {
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
        public ActionResult Delete(int id)
        {
            APIClient client = new APIClient(HttpContext);
            string skillSetJson = JsonConvert.SerializeObject(id);

            APIResponse response = client.CallAPI("POST", "/api/skillset/delete/" + id,
                new StringContent(skillSetJson, Encoding.UTF8, "application/json"));

            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, SkillSet skillSet)
        {
            APIClient client = new APIClient(HttpContext);
            string skillSetJson = JsonConvert.SerializeObject(skillSet);

            APIResponse response = client.CallAPI("POST", "/api/skillset/update/" + id,
                new StringContent(skillSetJson, Encoding.UTF8, "application/json"));

            return View();
        }

        public ActionResult Search(int id)
        {
            APIClient api = new APIClient(HttpContext);

            APIResponse response = api.CallAPI("GET", "/api/skillset/" + id);
            SkillSet skillSet = JsonConvert.DeserializeObject<SkillSet>(response.Content);

            response = api.CallAPI("GET", "/api/students?skillset=" + id);
            List<int> studentIds = JsonConvert.DeserializeObject<List<int>>(response.Content);
            IEnumerable<Student> students = studentIds.Select((studentId) => {
                response = api.CallAPI("GET", "/api/student/" + studentId);
                return JsonConvert.DeserializeObject<Student>(response.Content);
            });
            ViewData["Students"] = students;

            return View(skillSet);
        }


    }
}