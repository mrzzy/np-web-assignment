/*
 * Web Assignment 
 * Folio API
 * SkillSet API controller 
*/

using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using folio.Models;

namespace folio.Controllers
{
    // controller for the /skillset/ route
    public class SkillSetController : Controller
    {   
        /* Controller Routes */
       
        // route to query skillsets, with optional specification in url params:
        // name - filter the url parameter by exact match name
        // limit - limit results returned to the given no.
        // responds to request with the ids of all matching skillsets
        [HttpGet]
        [Produces("application/json")]
        public ActionResult Index([FromQuery] string name, [FromQuery] int? limit)
        {
            Console.WriteLine("Skillset: Index: name:" + name + " limit: " + limit.ToString());
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
    
    }
}
