/*
 * NP Web Assignment
 * Folio UI
*/

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

namespace folio_ui.Controllers
{     
    // ui controller: /student
    public class StudentController : Controller
    {
        // display student portfolio for the given id
        [HttpGet("/student/portfolio/{id}")]
        public ActionResult Portfolio(int id)
        {
            APIClient api = new APIClient(HttpContext);
            // add reference to api endpoint to view data
            ViewData["API_ENDPOINT"] = api.APIEndpoint;

            // pull student portfolio data for id
            APIResponse response = api.CallAPI("GET", "/api/student/portfolio/" + id);
            Student student = JsonConvert.DeserializeObject<Student>(response.Content);

            // pull student projects
            response = api.CallAPI("GET", "/api/projects?student=" + id);
            List<int> projectIds = JsonConvert.DeserializeObject<List<int>>(response.Content);
            IEnumerable<Project> projects = projectIds.Select((projectId) => 
            {
                response = api.CallAPI("GET", "/api/project/" + projectId);
                Project project = JsonConvert.DeserializeObject<Project>(response.Content);
                
                //clamp description down for rendering in small view
                int clampLimit = 200;
                if(project.Description.Count() > clampLimit) 
                {
                    project.Description = 
                        project.Description.Substring(0, clampLimit) + " ...";
                }

                return project;
            });
            ViewData["Projects"] = projects;

            // pull student's skilsets
            response = api.CallAPI("GET", "/api/skillsets?student=" + id);
            List<int> skillSetIds = JsonConvert.DeserializeObject<List<int>>(response.Content);
            IEnumerable<SkillSet> skillSets = skillSetIds.Select((skillSetId) => {
                response = api.CallAPI("GET", "/api/skillset/" + skillSetId);
                return JsonConvert.DeserializeObject<SkillSet>(response.Content);
            });
            ViewData["SkillSets"] = skillSets;
            
            return View(student);
        }
        
        // edit student profile for the student with the given id
        [HttpGet("/student/profile")]
        public ActionResult Profile()
        {
            APIClient api = new APIClient(HttpContext);
            // add reference to api endpoint to view data
            ViewData["API_ENDPOINT"] = api.APIEndpoint;
            return View();
        }
    }
}
