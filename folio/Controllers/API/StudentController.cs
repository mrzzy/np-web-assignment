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
    public class StudentController : Controller
    {
        [HttpGet("/api/student")]
        [Produces("application/json")]
        public ActionResult Query([FromQuery] string name, [FromQuery] int? skip, [FromQuery] int? limit)
        {
            // obtain the students that match the query
            List<int> matchIds = null;
            using (EPortfolioDB database = new EPortfolioDB())
            {
                IQueryable<Student> matchingStudents = database.Students;
                // apply filters (if any) in url parameters
                if (!string.IsNullOrWhiteSpace(name))
                {
                    matchingStudents = matchingStudents
                        .Where(s => s.Name == name);
                }
                if (skip != null && skip.Value >= 0)
                {
                    matchingStudents = matchingStudents
                        .Skip(skip.Value);
                }
                if (limit != null && limit.Value >= 0)
                {
                    matchingStudents = matchingStudents
                        .Take(limit.Value);
                }

                // convert matching students to there corresponding ids
                matchIds = matchingStudents.Select(s => s.StudentId).ToList();
            }

            return Json(matchIds);
        }

        [HttpGet("/api/student/{id}")]
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

        [HttpPost("/api/student/create")]
        [ValidateAntiForgeryToken]
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
            Object response = new { skillSetId = studentId };
            return Json(response);
        }

        [HttpPost("/api/student/update/{id}")]
        [ValidateAntiForgeryToken]
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

        [HttpPost("/api/student/delete/{id}")]
        [ValidateAntiForgeryToken]
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

        [HttpPost("/api/student/search/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult SearchStudent(int id)
        {
            using (EPortfolioDB database = new EPortfolioDB())
            {
                // Find the student specified by formModel
                Student student = database.Students
                    .Where(s => s.StudentId == id)
                    .Single();


            }

            return
        }
    }
}