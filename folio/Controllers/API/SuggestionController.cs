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
    public class SuggestionController : Controller
    {
        //View
        [HttpGet("/api/suggestions")]
        [Produces("application/json")]
        public ActionResult GetSuggestion()
        {
            List<Suggestion> suggestionList = null;
            using (EPortfolioDB db = new EPortfolioDB())
            {
                suggestionList = db.Suggestions.ToList();
            }
            return Json(suggestionList);
        }

        //Create
        [HttpPost("/api/suggestion/create")]
        public ActionResult PostSuggestion([FromBody] SuggestionFormModel formModel)
        {
            int suggestionId = -1;
            using (EPortfolioDB db = new EPortfolioDB())
            {
                Suggestion s = new Suggestion();
                formModel.Apply(s);

                db.Suggestions.Add(s);
                db.SaveChanges();
                suggestionId = s.SuggestionId;
            }

            Object response = new { suggestionId = suggestionId };
            return Json(response);
        }

        
        
    }
}
