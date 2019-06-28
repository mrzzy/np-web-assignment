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

namespace folio.API.Controllers
{
    // controller for the /api/skillset[s]/ route
    public class SkillSetController : Controller
    {   
        /* Controller Routes */
       
        // route to query skillsets, with optional specification in url params:
        // name - filter the url parameter by exact match name
        // limit - limit results returned to the given no.
        // responds to request with the ids of all matching skillsets
        [HttpGet("/api/skillsets")]
        [Produces("application/json")]
        public ActionResult Query([FromQuery] string name, [FromQuery] int? limit)
        {
            // obtain the skillsets that match the query
            List<int> matchIds = null;
            using(EPortfolioDB database = new EPortfolioDB())
            {
                IQueryable<SkillSet> matchingSkillsets = database.SkillSets;
                // apply filters (if any) in url parameters
                if(!string.IsNullOrWhiteSpace(name))
                {
                    matchingSkillsets = matchingSkillsets
                        .Where(s => s.SkillSetName == name);
                }
                if(limit != null && limit.Value >= 0)
                {
                    matchingSkillsets = matchingSkillsets
                        .Take(limit.Value);
                }
            
                // convert matching skillsets to there corresponding ids
                matchIds = matchingSkillsets.Select(s => s.SkillSetId).ToList();
            }
            
            return Json(matchIds);
        }
    
        // route to get a skillset for the given id
        // responds to request with json reprensetation of the skilset
        [HttpGet("/api/skillset/{id}")]
        [Produces("application/json")]
        public ActionResult GetSkillSet(int id)
        {
            Console.WriteLine("get id:", id.ToString());
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
        [HttpPost("/api/skillset/create")]
        [Produces("application/json")]
        public ActionResult CreateSkillSet([FromBody] SkillSetFormModel formModel)
        {
            // write the given skillset to database
            int skillSetId = -1;
            using(EPortfolioDB database = new EPortfolioDB())
            {
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
        [HttpPost("/api/skillset/update/{id}")]
        public ActionResult UpdateSkillSet(
                int id, [FromBody] SkillSetFormModel formModel)
        {
            using(EPortfolioDB database = new EPortfolioDB())
            {
                // Find the skillset specified by formModel
                SkillSet skillSet = database.SkillSets
                    .Where(s => s.SkillSetId == id)
                    .Single();

                // perform Update using data in form model
                formModel.Apply(skillSet);
                database.SaveChanges();
            }

            return Ok();
        }
    
        // route to delete skillset for skillset id
        [HttpPost("/api/skillset/delete/{id}")]
        public ActionResult DeleteSkillSet(int id)
        {
            using(EPortfolioDB database = new EPortfolioDB())
            {
                // Find the skillset specified by formModel
                SkillSet skillSet = database.SkillSets
                    .Where(s => s.SkillSetId == id)
                    .Single();
                
                // remove the skillSet from db
                database.SkillSets.Remove(skillSet);
                database.SaveChanges();
            }

            return Ok();
        }
    }
}
