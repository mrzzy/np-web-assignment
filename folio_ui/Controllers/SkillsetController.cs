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
    }
}