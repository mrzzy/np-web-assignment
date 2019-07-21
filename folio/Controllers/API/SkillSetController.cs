/*
 * Web Assignment 
 * Folio API
 * SkillSet API controller 
*/

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

namespace folio.API.Controllers
{
    // controller for the /api/skillset[s]/ route
    public class SkillSetController : Controller
    {   
        /* properties */
        private ActionResult SkillSetNameConflict 
        {
            get
            {
                return Conflict(new Dictionary<string, List<string>>
                {
                    { "SkillSetName", new List<string>{"Name of skillset conflicts with an existing skillset" }}
                });
            }
        }
        
        /* Controller Routes */
        // route to query skillsets, with optional specification in url params:
        // student - filter by student (given by id) whom have been assigned skillse
        // names - if 1 includes the name of the skillset in addition to ids
        // responds to request with the ids of all matching skillsets
        [HttpGet("/api/skillsets")]
        [Produces("application/json")]
        public ActionResult Query([FromQuery] int? student, 
                [FromQuery] int? names)
        {
            // obtain the skillsets that match the query
            using(EPortfolioDB database = new EPortfolioDB())
            {
                IQueryable<SkillSet> matchingSkillsets = database.SkillSets;
                
                // apply filters (if any) in url parameters
                if(student != null)
                {
                    // select only skillsets assigned to student
                    matchingSkillsets = matchingSkillsets
                        .Include(s => s.StudentSkillSets)
                        .Where(s => s.StudentSkillSets
                            .Any(ss => ss.StudentId == student));
                }
            
                // contruct response
                if(names != null && names.Value == 1)
                {
                    List<Object> results = matchingSkillsets
                    .Select(s => new 
                    {
                        Id = s.SkillSetId,
                        Name = s.SkillSetName
                    } as Object ).ToList();
                    
                    return Json(results);
                }
                else // ids only
                {
                    List<int> matchIds = matchingSkillsets
                        .Select(s => s.SkillSetId).ToList();
                    return Json(matchIds);
                }
            }
        }

        // route to get a skillset for the given id
        // responds to request with json reprensetation of the skilset
        [HttpGet("/api/skillset/{id}")]
        [Produces("application/json")]
        public ActionResult GetSkillSet(int id)
        {
            // Retrieve the Skillset for id
            SkillSet skillset = null;
            using(EPortfolioDB database = new EPortfolioDB())
            {
                skillset = database.SkillSets
                    .Where(s => s.SkillSetId == id)
                    .FirstOrDefault();
            }
        
            // check if skill has been found for targetId
            if(skillset == null) return NotFound();

            return Json(skillset);
        }
            
        // route to create a skillset for skillset form model
        // responds to request with json representatoin of the skillset
        // authentication lecturer is required
        [HttpPost("/api/skillset/create")]
        [Produces("application/json")]
        [Authenticate("Lecturer")]
        public ActionResult CreateSkillSet([FromBody] SkillSetFormModel formModel)
        {
            // check if contents of form model is valid
            if(!ModelState.IsValid)
            { return BadRequest(ModelState); }
            
            // write the given skillset to database
            int skillSetId = -1;
            using(EPortfolioDB database = new EPortfolioDB())
            {
                // check if skillset name does not conflict with existing skillset
                if(database.SkillSets
                    .Where(s => s.SkillSetName == formModel.SkillSetName)
                    .Count() >= 1)
                { return SkillSetNameConflict; }

                // create skillSet with form model values
                SkillSet skillSet = new SkillSet();
                formModel.Apply(skillSet);

                // add new skillset to database
                database.SkillSets.Add(skillSet);
                database.SaveChanges();
                skillSetId = skillSet.SkillSetId;
            }

            // respond with sucess message with inserted skillset id
            Object response = new { skillSetId = skillSetId };
            return Json(response);
        }
    
        // route to update skillset for skillset id and skillset form model
        // authentication lecturer is required
        [HttpPost("/api/skillset/update/{id}")]
        [Authenticate("Lecturer")]
        public ActionResult UpdateSkillSet(
                int id, [FromBody] SkillSetFormModel formModel)
        {
            // check if contents of form model is valid
            if(!ModelState.IsValid)
            { return BadRequest(ModelState); }

            using(EPortfolioDB database = new EPortfolioDB())
            {
                // check if skillset name does not conflict with existing skillset
                if(database.SkillSets
                    .Where(s => s.SkillSetName == formModel.SkillSetName)
                    .Count() >= 2) // all 1 match since updating
                { return SkillSetNameConflict; }

                // Find the skillset specified by formModel
                SkillSet skillSet = database.SkillSets
                    .Where(s => s.SkillSetId == id)
                    .FirstOrDefault();
                if(skillSet == null) return NotFound();

                // perform Update using data in form model
                formModel.Apply(skillSet);
                database.SaveChanges();
            }

            return Ok();
        }
    
        // route to delete skillset for skillset id
        // cascade deletes any skillset assignments to student (StudentSkillSet)
        // authentication for lecturer is required
        [HttpPost("/api/skillset/delete/{id}")]
        [Authenticate("Lecturer")]
        public ActionResult DeleteSkillSet(int id)
        {
            using(EPortfolioDB database = new EPortfolioDB())
            {
                // cascade delete any StudentSkillSet assignments
                IQueryable<StudentSkillSet> assignments = database.StudentSkillSets
                    .Where(s => s.SkillSetId == id);
                database.StudentSkillSets.RemoveRange(assignments);
            
                // Find the skillset specified by formModel
                SkillSet skillSet = database.SkillSets
                    .Where(s => s.SkillSetId == id)
                    .FirstOrDefault();
                if(skillSet == null) return NotFound();
                
                // remove the skillSet from db
                database.SkillSets.Remove(skillSet);
                database.SaveChanges();
            }

            return Ok();
        }
    
        // route to assign the skillset with the specified id to the student 
        // with the specified student id in query
        [HttpPost("/api/skillset/assign/{id}")]
        [Authenticate("Student")]
        public ActionResult AssignSkillSet(int id, [FromQuery] int student)
        {
            using(EPortfolioDB database = new EPortfolioDB())
            {
                // check if student skillset already exists in the database
                IQueryable<StudentSkillSet> matchingAssignments = database
                    .StudentSkillSets
                        .Where(s => s.SkillSetId == id)
                        .Where(s => s.StudentId == student);
                if(matchingAssignments.Count() >= 1)
                    return Ok(); // skillset already assigned to student
            
                // obtain models for the specified by the given request
                SkillSet skillSetModel = database.SkillSets  
                    .Where(s => s.SkillSetId == id).FirstOrDefault();
                Student studentModel = database.Students
                    .Where(s => s.StudentId == student).FirstOrDefault();
            
                if(skillSetModel == null || studentModel == null)
                { return NotFound(); }
                
                // assign the skillset to the student
                StudentSkillSet assignment = new StudentSkillSet
                {
                    Student = studentModel,
                    SkillSet = skillSetModel
                };
                database.StudentSkillSets.Add(assignment);
                database.SaveChanges();
            } 

            return Ok();
        }
    
        // route to remove  the skillset with the specified id to the student 
        // with the specified student id in query
        [HttpPost("/api/skillset/remove/{id}")]
        [Authenticate("Student")]
        public ActionResult RemoveSkillSet(int id, [FromQuery] int student)
        {
            using(EPortfolioDB database = new EPortfolioDB())
            {
                // obtain assignment as per the given request 
                StudentSkillSet assignment  = database.StudentSkillSets
                    .Where(s => s.SkillSetId == id)
                    .Where(s => s.StudentId == student)
                    .FirstOrDefault();
                if(assignment == null) return NotFound();
                
                // remove assignment from database
                database.StudentSkillSets.Remove(assignment);
                database.SaveChanges();
            }

            return Ok();
        }
    }
}
