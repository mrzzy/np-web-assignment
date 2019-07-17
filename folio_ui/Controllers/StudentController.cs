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
            // pull student portfolio data for id
            APIClient apiClient = new APIClient();
            string portfolioJson = apiClient .CallAPI("GET", 
                    "/api/student/portfolio/" + id);
            Student student = JsonConvert.DeserializeObject<Student>(portfolioJson);
            return View(student);
        }
    }
}
