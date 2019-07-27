using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DotNetEnv;

using folio.Models;
using folio.FormModels;
using folio.Services.Auth;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace folio.Controllers.API
{
    [Route("api/[controller]")]
    public class LecturerController : Controller
    {

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

        [HttpGet("/api/lecturers/details")]
        [Produces("application/json")]
        public ActionResult LecturersDetails()
        {
            List<Lecturer> l = new List<Lecturer>();
            using(EPortfolioDB db = new EPortfolioDB())
            {
                l = db.Lecturers.ToList();
                db.SaveChanges();
            }
            return Json(l);
        }

        // GET: api/lecturers
        // route to get lectuer ids, governed by optional query parameters:
        // student - filter to only those whom mentor the student by given student id
        // names - if 1, will include names of lectuers together with response
        [HttpGet("/api/lecturers")]
        [Produces("application/json")]
        public ActionResult GetLecturers([FromQuery] int? student, 
                [FromQuery] int? names)
        {
            using (EPortfolioDB db = new EPortfolioDB())
            {
                IQueryable<Lecturer> matchingLecturers = db.Lecturers;

                if (student != null)
                {
                    matchingLecturers = matchingLecturers
                        .Include(l => l.Students)
                        .Where(l => l.Students.Any(ll => ll.StudentId == student));
                }

                // construct response with matching lecturers
                if(names != null && names.Value == 1) // id + names
                {
                    List<Object> results = matchingLecturers
                    .Select(l => new 
                    {
                        Id = l.LecturerId,
                        Name = l.Name
                    } as Object ).ToList();
                    
                    return Json(results);
                }
                else // only ids
                {
                    List<int> results = matchingLecturers
                        .Select(l => l.LecturerId).ToList();
                    return Json(results);
                }
            }
        }

        // GET api/lecturer/5
        [HttpGet("/api/lecturer/{id}")]
        [Produces("application/json")]
        //[Authenticate("Lecturer")]
        public ActionResult GetLectureById(int id)
        {
            Console.WriteLine("get id:", id.ToString());
            // Retrieve the lecturer for id
            Lecturer lecturer = null;
            using (EPortfolioDB database = new EPortfolioDB())
            {
                lecturer = database.Lecturers
                    .Where(l => l.LecturerId == id)
                    .FirstOrDefault();
            }

            // check if skill has been found for targetId
            if (lecturer == null) return NotFound();

            return Json(lecturer);
        }

        //POST api/lecturer/create
        [HttpPost("/api/lecturer/create")]
        [Produces("application/json")]
        //[Authenticate("Lecturer")]
        public ActionResult CreateLecture([FromBody] LecturerCreateFormModel fm)
        {

            if (!ModelState.IsValid)
            { return BadRequest(ModelState); }

            // check if existing user already has email address
            if (AuthService.FindUser(fm.EmailAddr) != null)
            { return EmailAddrConflict; }


            int lecturerId = -1;            
            using (EPortfolioDB db = new EPortfolioDB())
            {
                Lecturer lecturer = fm.Create();
                db.Lecturers.Add(lecturer);
                db.SaveChanges();

                lecturerId = lecturer.LecturerId;

            }

            Object response = new { lecturerId = lecturerId };
            return Json(response);

        }

        // route to update Lecturers for LectureId and Lecturer form model
        // authentication lecturer is required
        [HttpPost("/api/lecturer/update/{id}")]
        //[Authenticate("Lecturer")]
        public ActionResult UpdateLecturer(
                int id, [FromBody] LecturerUpdateFormModel formModel)
        {
            if (!ModelState.IsValid)
            { return BadRequest(ModelState); }

            using (EPortfolioDB database = new EPortfolioDB())
            {
                // Find the lecturer specified by formModel
                Lecturer lecturer = database.Lecturers
                    .Where(s => s.LecturerId == id)
                    .Single();
                if (lecturer == null)
                { return NotFound(); }

                //Session session = AuthService.ExtractSession(HttpContext);
                //if (session.MetaData["UserRole"] != "Lecturer" && // any lecturer
                //     session.EmailAddr != lecturer.EmailAddr) // this student
                //{ return Unauthorized(); }

                // perform Update using data in form model
                formModel.Apply(lecturer);
                database.SaveChanges();
            }

            return Ok();
        }

        // route to delete lecturer for lecturer id
        [HttpPost("/api/lecturer/delete/{id}")]
        [Produces("application/json")]
        //[Authenticate("Lecturer")]
        public ActionResult DeleteLecturer(int id)
        {
            using (EPortfolioDB database = new EPortfolioDB())
            {
                // Find the lecturer specified by formModel
                Lecturer lecturer = database.Lecturers
                    .Where(l => l.LecturerId == id)
                    .Single();

                if (lecturer == null)
                { return NotFound(); }

                //Session session = AuthService.ExtractSession(HttpContext);
                //if (session.MetaData["userrole"] != "lecturer" && // any lecturer
                //     session.EmailAddr != lecturer.EmailAddr) // this student
                //{ return Unauthorized(); }

                // remove the skillSet from db
                database.Lecturers.Remove(lecturer);
                database.SaveChanges();
            }

            return Ok();
        }

        // GET api/lecturer/mentees/5/
        [HttpGet("/api/lecturer/mentees/{id}")]
        [Produces("application/json")]
        //[Authenticate("Lecturer")]
        public ActionResult GetMentees(int id)
        {
            Console.WriteLine("get id:", id.ToString());
            // Retrieve the lecturer for id
            List<Student> studentList = new List<Student>();
            using (EPortfolioDB database = new EPortfolioDB())
            {               
                studentList = database.Students
                    .Where(s => s.MentorId == id).ToList();
                database.SaveChanges();
                    
            }

            // check if skill has been found for targetId
            if (studentList == null) return NotFound();

            return Json(studentList);
        }


        [HttpPost("/api/lecturer/changePW/{id}")]
        //[Authenticate("Lecturer")]
        public ActionResult ChangeLecturerPassword(
                int id, [FromBody] LecturerPasswordFormModel formModel)
        {
            if (!ModelState.IsValid)
            { return BadRequest(ModelState); }

            using (EPortfolioDB database = new EPortfolioDB())
            {
                // Find the lecturer specified by formModel
                Lecturer lecturer = database.Lecturers
                    .Where(s => s.LecturerId == id)
                    .Single();
                if (lecturer == null)
                { return NotFound(); }


                //Session session = AuthService.ExtractSession(HttpContext);
                //if (session.MetaData["UserRole"] != "Lecturer" && // any lecturer
                //     session.EmailAddr != lecturer.EmailAddr) // this student
                //{ return Unauthorized(); }

                // perform Update using data in form model
                formModel.Apply(lecturer);
                database.SaveChanges();
            }

            return Ok();
        }
    }
}
