using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using Newtonsoft.Json;

using folio.Models;
using folio.FormModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace folio.Controllers.API
{
    [Route("api/[controller]")]
    public class SuggestionController : Controller
    {
        //View
        [HttpGet("/api/suggestions")]
        [Produces("application/json")]
        public ActionResult GetSuggestion()
        {
            List<int> suggestionIds = null;
            using (EPortfolioDB db = new EPortfolioDB())
            {
                suggestionIds = db.Suggestions
                    .Select(s => s.SuggestionId).ToList();
            }
            return Json(suggestionIds);
        }

        //Create
        [HttpPost("/api/suggestion/create")]
        [Produces("application/json")]
        //[Authenticate("Lecturer")]
        public ActionResult PostSuggestion([FromBody] SuggestionFormModel formModel)
        {
            EPortfolioDB context = new EPortfolioDB();
            bool ifStudentExist = false;//check if StudentID is valid
            bool ifLecturerExist = false;//check if LecturerID is valid
            int suggestionId = -1;
            Suggestion s = new Suggestion();
            formModel.Apply(s);
            TryValidateModel(s);
            if (ModelState.IsValid)
            {
                //Validate if the StudentId and LecturerId is existed
                foreach (Lecturer i in context.Lecturers)
                {
                    if (s.LecturerId == i.LecturerId)
                    {
                        ifLecturerExist = true;
                        break;
                    }
                    else
                    {
                        ifLecturerExist = false;
                        
                    }
                }
                foreach (Student i in context.Students)
                {
                    if (s.StudentId == i.StudentId)
                    {
                        ifStudentExist = true;
                        break;
                    }
                    else
                    {
                        ifStudentExist = false;
                        
                    }
                }
                using (EPortfolioDB db = new EPortfolioDB())
                {
                    //If both StudentId & LecturerID is existed DB will save changes
                    if(ifStudentExist == true && ifLecturerExist == true)
                    {
                        db.Suggestions.Add(s);
                        db.SaveChanges();
                        suggestionId = s.SuggestionId;
                    }
                  
                }
                Object response = new { suggestionId = suggestionId };
                return Json(response);
            }
            else
            {
                return NotFound();
            }
        }

        // GET api/suggestion/5
        [HttpGet("/api/suggestion/{id}")]
        [Produces("application/json")]
        //[Authenticate("Lecturer")]
        public ActionResult GetLectureById(int id)
        {
            Console.WriteLine("get id:", id.ToString());
            // Retrieve the suggestion for id
            Suggestion suggestion = null;
            using (EPortfolioDB database = new EPortfolioDB())
            {
                suggestion = database.Suggestions
                    .Where(l => l.SuggestionId == id)
                    .FirstOrDefault();
            }

            if (suggestion == null) return NotFound();

            return Json(suggestion);
        }

        // POST delete suggestion
        [HttpPost("/api/suggestion/delete/{id}")]
        [Produces("application/json")]
        //[Authenticate("Lecturer")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteSuggestion(int id)
        {
            using (EPortfolioDB database = new EPortfolioDB())
            {
                // Find the lecturer specified by formModel
                Suggestion suggestion = database.Suggestions
                    .Where(l => l.SuggestionId == id)
                    .Single();

                // remove the skillSet from db
                database.Suggestions.Remove(suggestion);
                database.SaveChanges();
            }

            return Ok();
        }

        //Acknowledge suggestions
        [HttpGet("/api/suggestion/ack/{id}")]
        [Produces("application/json")]
        public ActionResult Acknowledge(int id)
        {
            Suggestion suggestion = null;
            using (EPortfolioDB db = new EPortfolioDB())
            {
                 suggestion = db.Suggestions
                    .Where(s => s.SuggestionId == id)
                    //.Include(s => s.Student)
                    .Single();
                    
            }
            return Json(suggestion);
        }
        
    }
}
