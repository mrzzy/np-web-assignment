using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

using folio.Models;
using folio.FormModels;

namespace folio.Controllers.API
{
    /* Controller for the /api/students route */
    public class StudentController : Controller
    {
        /* API routes */
        // route to get student ids, governed by optional query parameters:
        // skillset - filter by skillset (given by id) assigned (StudentSkillSet)
        [HttpGet("/api/students")]
        [Produces("application/json")]
        public ActionResult GetStudents([FromQuery] int? skillset)
        {
            List<int> studentIds = null;
            using (EPortfolioDB db = new EPortfolioDB())
            {
                IQueryable<Student> matchingStudents = db.Students;
                
                // apply filters if specified
                if(skillset != null)
                {
                    // filter by skillset id 
                    matchingStudents = matchingStudents
                        .Include(s => s.StudentSkillSets)
                            .ThenInclude(ss => ss.SkillSet)
                        .Where(s => s.StudentSkillSets
                                .Any(ss => ss.SkillSet.SkillSetId == skillset));
                }
                
                studentIds = matchingStudents.Select(s => s.StudentId).ToList();
            }
            return Json(studentIds);
        }

        [HttpGet("/api/students/{id}")]
        [Produces("application/json")]
        public ActionResult GetStudent(int id)
        {
            Console.WriteLine("get id:", id.ToString());
            // Retrieve the Student for id
            Student student = null;
            using (EPortfolioDB database = new EPortfolioDB())
            {
                student = database.Students
                    .Where(s => s.StudentId == id)
                    .FirstOrDefault();
            }

            // check if student has been found for targetId
            if (student == null) return NotFound();

            return Json(student);
        }

        //Create new student
        [HttpPost("/api/students/create")]
        [Produces("application/json")]
        public ActionResult CreateStudent([FromBody] StudentFormModel formModel)
        {
            // write the given student to database
            int studentId = -1;
            using (EPortfolioDB database = new EPortfolioDB())
            {
                // create student with form model values
                Student student = new Student();
                formModel.Apply(student);

                // add new student to database
                database.Students.Add(student);
                database.SaveChanges();
                studentId = student.StudentId;
            }

            // respond with sucess message with inserted student id
            Object response = new { Id = studentId };
            return Json(response);
        }

        //Update student
        [HttpPost("/api/students/update/{id}")]
        public ActionResult UpdateStudent(
                int id, [FromBody] StudentFormModel formModel)
        {
            using (EPortfolioDB database = new EPortfolioDB())
            {
                // Find the student specified by formModel
                Student student = database.Students
                    .Where(s => s.StudentId == id)
                    .Single();

                // perform Update using data in form model
                formModel.Apply(student);
                database.SaveChanges();
            }

            return Ok();
        }

        //Delete student
        [HttpPost("/api/students/delete/{id}")]
        public ActionResult DeleteStudent(int id)
        {
            using (EPortfolioDB database = new EPortfolioDB())
            {
                // Find the student specified by formModel
                Student student = database.Students
                    .Where(s => s.StudentId == id)
                    .Single();

                // remove the student from db
                database.Students.Remove(student);
                database.SaveChanges();
            }

            return Ok();
        }
    }
}
