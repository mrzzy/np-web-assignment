﻿using System;
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
                return Conflict(new Dictionary<string, List<string>>
                {
                    { "EmailAddr", new List<string>{ "User with Email adddress already exists." }}
                });
            }
        }

        /* API routes */
        // route to get student ids, governed by optional query parameters:
        // skillset - filter by skillset (given by id) assigned (StudentSkillSet)
        // project - filter by project (given by id) assigned (ProjectMember)
        // names - if 1, will include names of students together with response
        [HttpGet("/api/students")]
        [Produces("application/json")]
        public ActionResult GetStudents([FromQuery] int? skillset, 
                [FromQuery]int? names, [FromQuery]int? project)
        {
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
                    // filter by project id 
                    matchingStudents = matchingStudents
                        .Include(s => s.ProjectMembers)
                        .Where(s => s.ProjectMembers
                            .Any(pm => pm.ProjectId == project));
                }
            
                // construct response with matching students
                if(names != null && names.Value == 1) // id + names of students
                {
                    List<Object> results = matchingStudents
                    .Select(s => new 
                    {
                        Id = s.StudentId,
                        Name = s.Name
                    } as Object ).ToList();
                    
                    return Json(results);
                }
                else // ids only
                { 
                    List<int> results = matchingStudents
                        .Select(s => s.StudentId).ToList();
                    return Json(results);
                }
            }
        }
    
        // route to get student portfolio infomation for given student id
        // compared to GetStudent() this route does not require authentication
        // but limits student infomation responded
        [HttpGet("/api/student/portfolio/{id}")]
        [Produces("application/json")]
        public ActionResult GetStudentPortfolio(int id)
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
        
            // extract portfolio information from student
            Object portfolio = new 
            {
                StudentId = student.StudentId,
                Name = student.Name,
                Course = student.Course,
                Photo = student.Photo,
                Description = student.Description,
                Achievement = student.Achievement,
                ExternalLink = student.ExternalLink,
                EmailAddr = student.EmailAddr,
                MentorId = student.MentorId
            };
        
            return Json(portfolio);
        }

        // route to get student for given student id
        // authentication required: only lecturers or the specific student
        // (the student being updated) is allowed to update the student
        // on validation failure responses with validation errors
        [Authenticate]
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

            // Check authorized to perform view student
            Session session = AuthService.ExtractSession(HttpContext);
            if(session.MetaData["UserRole"] != "Lecturer" && // any lecturer
                 session.EmailAddr != student.EmailAddr) // this student
            { return Unauthorized(); }

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
    }
}
