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
        //GET: api/Lecturers
        [HttpGet("/api/Lecturers")]
        [Produces("application/json")]
        public ActionResult Query(
               [FromQuery] string name, [FromQuery] int? skip, [FromQuery] int? limit)
        {
            // obtain the skillsets that match the query
            List<int> matchIds = null;
            using (EPortfolioDB database = new EPortfolioDB())
            {
                IQueryable<Lecturer> matchingLecturers = database.Lecturers;
                // apply filters (if any) in url parameters
                if (!string.IsNullOrWhiteSpace(name))
                {
                    matchingLecturers = matchingLecturers
                        .Where(s => s.Name == name);
                }
                if (skip != null && skip.Value >= 0)
                {
                    matchingLecturers = matchingLecturers
                        .Skip(skip.Value);
                }
                if (limit != null && limit.Value >= 0)
                {
                    matchingLecturers = matchingLecturers
                        .Take(limit.Value);
                }

                // convert matching skillsets to there corresponding ids
                matchIds = matchingLecturers.Select(s => s.LecturerId).ToList();
            }

            return Json(matchIds);
        }


        // GET: api/lecturers/details
        [HttpGet("/api/lecturers/details")]
        [Produces("application/json")]
        public ActionResult GetLecturers()
        {
            List<Lecturer> lecturerlist = null;
            using (EPortfolioDB db = new EPortfolioDB())
            {                
                lecturerlist = db.Lecturers.ToList();
                
            }
            return Json(lecturerlist);
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
        public ActionResult CreatLecture([FromBody] LecturerFormModel fm)
        {
            int lecturerId = -1;
            Lecturer lecturer = new Lecturer();
            fm.Apply(lecturer);
            TryValidateModel(lecturer);
            if (ModelState.IsValid)
            {
                using (EPortfolioDB db = new EPortfolioDB())
                {

                    db.Lecturers.Add(lecturer);
                    db.SaveChanges();

                    lecturerId = lecturer.LecturerId;

                }

                Object response = new { lecturerId = lecturerId };
                return Json(response);
            }
            else
            {
                return NotFound();
            }

        }

        // route to update Lecturers for LectureId and Lecturer form model
        // authentication lecturer is required
        [HttpPost("/api/lecturer/update/{id}")]
        //[Authenticate("Lecturer")]
        public ActionResult UpdateLecturer(
                int id, [FromBody] LecturerFormModel formModel)
        {
            using (EPortfolioDB database = new EPortfolioDB())
            {
                // Find the lecturer specified by formModel
                Lecturer lecturer = database.Lecturers
                    .Where(s => s.LecturerId == id)
                    .Single();

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
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteLecturer(int id)
        {
            using (EPortfolioDB database = new EPortfolioDB())
            {
                // Find the lecturer specified by formModel
                Lecturer lecturer = database.Lecturers
                    .Where(l => l.LecturerId == id)
                    .Single();

                // remove the skillSet from db
                database.Lecturers.Remove(lecturer);
                database.SaveChanges();
            }

            return Ok();
        }

    }
}
