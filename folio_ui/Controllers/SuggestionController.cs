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
using System.Text;

namespace folio_ui.Controllers
{
    public class SuggestionController : Controller
    {
        // GET: Suggestions
        public ActionResult Index(int id)
        {
            APIClient api = new APIClient(HttpContext);
            APIResponse response = api.CallAPI("GET", "/api/lecturer/mentees/" + id);
            List<Student> lecturer = JsonConvert.DeserializeObject<List<Student>>(response.Content);

            return View(lecturer);

            
        }

        // GET: Suggestion/Details/5
        public ActionResult Details(int id)
        {
            APIClient api = new APIClient(HttpContext);
            APIResponse response = api.CallAPI("GET", "/api/suggestion/student/" + id);
            List<Suggestion> suggestionList = JsonConvert.DeserializeObject<List<Suggestion>>(response.Content);

            ViewData["studentId"] = id;
            return View(suggestionList);

            
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
        public ActionResult Create(Suggestion suggestion)
        {
            var content = JsonConvert.SerializeObject(suggestion);
            string contentType = "application/json";

            var sContent = new StringContent(content, Encoding.UTF8, contentType);

            APIClient api = new APIClient(HttpContext);

            APIResponse response = api.CallAPI("POST", "/api/suggestion/create", sContent);

            return RedirectToAction("Index", "Lecturer");

            
        }        
       
    }
}

