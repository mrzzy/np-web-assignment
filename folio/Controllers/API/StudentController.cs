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
        //View students from a list
        [HttpGet("/api/students")]
        [Produces("application/json")]
        public ActionResult GetStudents()
        {
            List<Student> studentList = null;
            using (EPortfolioDB db = new EPortfolioDB())
            {
                studentList = db.Students.ToList();

            }
            return Json(studentList);
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

            // respond with success message with inserted student id
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

        //Search student by skillset
        [HttpGet("/api/students/search")]
        [Produces("application/json")]
        public ActionResult Query([FromQuery] string name)
        {
            // obtain the skillsets that match the query
            List<int> matchIds = null;
            List<int> searchedIds = new List<int>();
            List<List<string>> searchedStudents = new List<List<string>>();
            using (EPortfolioDB database = new EPortfolioDB())
            {
                IQueryable<SkillSet> matchingSkillsets = database.SkillSets;
                // apply filters (if any) in url parameters
                if (!string.IsNullOrWhiteSpace(name))
                {
                    matchingSkillsets = matchingSkillsets
                        .Where(s => s.SkillSetName == name);
                }

                // convert matching skillsets to there corresponding ids
                matchIds = matchingSkillsets.Select(s => s.SkillSetId).ToList();

                List<StudentSkillSet> studentSkillSet = database.StudentSkillSets
                    .Where(s => s.SkillSetId == matchIds[0])
                    .ToList();

                System.Diagnostics.Debug.WriteLine(studentSkillSet);

                //searchedIds.Add(studentSkillSet.StudentId);

                List<string> studentInfo = new List<string>();

                foreach (StudentSkillSet ss in studentSkillSet)
                {
                    searchedIds.Add(ss.StudentId);
                }

                for (int i = 0; i < searchedIds.Count; i++)
                {
                    Student student = database.Students
                        .Where(s => s.StudentId == searchedIds[i])
                        .Single();

                    studentInfo.Add(student.Name);
                    studentInfo.Add(student.StudentId.ToString());
                    studentInfo.Add(student.ExternalLink);
                }

                searchedStudents.Add(studentInfo);

                return Json(searchedStudents);
            }
        }        
        //Change profile picture

    }
}