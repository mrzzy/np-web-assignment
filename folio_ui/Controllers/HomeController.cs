using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using folio.Services.API;
using folio_ui.Models;
using folio.Models;

namespace folio_ui.Controllers
{
    public class HomeController : Controller
    {
        // Landing page of the side
        public IActionResult Index()
        {
            APIClient client = new APIClient();
            // pull list of students to display
            APIResponse response = client.CallAPI("GET", "/api/students");
            List<int> studentIds = JsonConvert.DeserializeObject<List<int>>(response.Content);
            List<Student> students = new List<Student>();
            foreach (int studentId in studentIds)
            {
                // pull student for student id
                response = client.CallAPI("GET", "/api/student/portfolio/" + studentId);
                Student student = JsonConvert.DeserializeObject<Student>(response.Content);
                students.Add(student);
                
                //clamp description down for rendering in small view
                int clampLimit = 200;
                if(student.Description.Count() > clampLimit) 
                {
                    student.Description = 
                        student.Description.Substring(0, clampLimit) + " ...";
                }
            }

            return View(students);
        }
    
        // Server Error Page
        [ResponseCache(
                Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id 
                    ?? HttpContext.TraceIdentifier });
        }
    }
}
