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
    public class LecturerController : Controller
    {        

        // GET: api/lecturers
        [HttpGet("/api/lecturers")]
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
        public ActionResult CreatLecture([FromBody] LecturerFormModel fm)
        {
            int lecturerId = -1;
            using (EPortfolioDB db = new EPortfolioDB())
            {
                Lecturer lecturer = new Lecturer();
                fm.Apply(lecturer);

                db.Lecturers.Add(lecturer);
                db.SaveChanges();

                lecturerId = lecturer.LecturerId;

            }

            Object response = new { lecturerId = lecturerId };
            return Json(response);
        }

        // route to delete lecturer for lecturer id
        [HttpPost("/api/lecturer/delete/{id}")]
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
