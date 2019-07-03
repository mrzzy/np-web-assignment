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
using folio.Services.Auth;

namespace folio.Controllers.API
{
    /* Controller for the /api/students route */
    public class StudentController : Controller
    {
        /* properties */
        private ActionResult EmailAddrConflict 
        {
            get 
            {
                return Conflict(new Dictionary<string, string>
                {
                    { "EmailAddr", "User with Email adddress already exists." }
                });
            }
        }

        /* API routes */
        // route to get student ids, governed by optional query parameters:
        // skillset - filter by skillset (given by id) assigned (StudentSkillSet)
        // project - filter by project (given by id) assigned (ProjectMember)
        [HttpGet("/api/students")]
        [Produces("application/json")]
        public ActionResult GetStudents([FromQuery] int? skillset, int? project)
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
                        .Where(s => s.StudentSkillSets
                            .Any(ss => ss.SkillSetId == skillset));
                }
            
                if(project != null)
                {
                    // filter by skillset id 
                    matchingStudents = matchingStudents
                        .Include(s => s.ProjectMembers)
                        .Where(s => s.ProjectMembers
                            .Any(pm => pm.ProjectId == project));
                }
            
                studentIds = matchingStudents.Select(s => s.StudentId).ToList();
            }
            return Json(studentIds);
        }

        // route to get student for given student id
        [HttpGet("/api/student/{id}")]
        [Produces("application/json")]
        public ActionResult GetStudent(int id)
        {
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

        // route to create a student for student form model
        // on success, responses with id of newly created student form model
        // on validation failure  responses with validation errors
        [HttpPost("/api/student/create")]
        [Produces("application/json")]
        public ActionResult CreateStudent(
                [FromBody] StudentCreateFormModel formModel)
       {
            // check if contents of form model is valid
            if(!ModelState.IsValid)
            { return BadRequest(ModelState); }

            // check if existing user already has email address
            if(AuthService.FindUser(formModel.EmailAddr) != null)
            { return EmailAddrConflict; }

            // write the given student to database
            int studentId = -1;
            using (EPortfolioDB database = new EPortfolioDB())
            {
                // create student with form model values
                Student student = formModel.Create();

                // add new student to database
                database.Students.Add(student);
                database.SaveChanges();
                studentId = student.StudentId;
            }

            // respond with success message with inserted student id
            Object response = new { Id = studentId };
            return Json(response);
        }

        // update student with the given id with data from the student form model
        // authentication required: only lecturers or the specific student
        // (the student being updated) is allowed to update the student
        // on validation failure responses with validation errors
        [HttpPost("/api/student/update/{id}")]
        [Authenticate()]
        public ActionResult UpdateStudent(
                int id, [FromBody] StudentUpdateFormModel formModel)
        {
            // check if contents of form model is valid
            if(!ModelState.IsValid)
            { return BadRequest(ModelState); }
            
            using (EPortfolioDB database = new EPortfolioDB())
            {
                // Find the student specified by formModel
                Student student = database.Students
                    .Where(s => s.StudentId == id)
                    .FirstOrDefault();
                if(student == null)
                { return NotFound(); }

                // Check authorized to perform update
                Session session = AuthService.ExtractSession(HttpContext);
                if(session.MetaData["UserRole"] != "Lecturer" && // any lecturer
                     session.EmailAddr != student.EmailAddr) // this student
                { return Unauthorized(); }

                // perform Update using data in form model
                formModel.Apply(student);
                database.SaveChanges();
            }

            return Ok();
        }

        // route to delete the student the given id 
        // cascade delete any StudentSkillSets assigned
        // authentication required: only lecturers or the specific student
        // (the student being deleted) is allowed to delete the student
        [HttpPost("/api/student/delete/{id}")]
        [Authenticate()]
        public ActionResult DeleteStudent(int id)
        {
            using (EPortfolioDB database = new EPortfolioDB())
            {
                // Find the student specified by formModel
                Student student = database.Students
                    .Where(s => s.StudentId == id)
                    .Include(s => s.StudentSkillSets)
                    .FirstOrDefault();
                if(student == null)
                { return NotFound(); }

                // cascade delete the students skillsets
                database.StudentSkillSets.RemoveRange(student.StudentSkillSets);
            
                // check authorized to perform deletion
                Session session = AuthService.ExtractSession(HttpContext);
                if(session.MetaData["UserRole"] != "Lecturer" && // any lecturer
                     session.EmailAddr != student.EmailAddr) // this student
                { return Unauthorized(); }

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
