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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace folio.Controllers.API
{
    public class ProjectController : Controller
    {
        /* Controller Routes */

        // route to query projects, with optional specification in url params:
        // name - filter the url parameter by exact match name
        // limit - limit results returned to the given no.
        // responds to request with the ids of all matching skillsets
        [HttpGet("/api/projects")]
        [Produces("application/json")]
        public ActionResult Query([FromQuery] string name, [FromQuery] int? limit)
        {
            // obtain the projects that match the query
            List<int> matchIds = null;
            using (EPortfolioDB database = new EPortfolioDB())
            {
                IQueryable<Project> matchingProjects = database.Projects;
                // apply filters (if any) in url parameters
                if (!string.IsNullOrWhiteSpace(name))
                {
                    matchingProjects = matchingProjects
                        .Where(s => s.Title == name);
                }
                if (limit != null && limit.Value >= 0)
                {
                    matchingProjects = matchingProjects
                        .Take(limit.Value);
                }

                // convert matching project to there corresponding ids
                matchIds = matchingProjects.Select(s => s.ProjectId).ToList();
            }

            return Json(matchIds);
        }
        [HttpGet("/api/project/{id}")]
        [Produces("application/json")]
        public ActionResult GetProject(int id)
        {
            Console.WriteLine("get id:", id.ToString());
            // Retrieve the Project for id
            Project project = null;
            using (EPortfolioDB database = new EPortfolioDB())
            {
                project = database.Projects
                    .Where(s => s.ProjectId == id)
                    .FirstOrDefault();
            }

            // check if project has been found for targetId
            if (project == null) return NotFound();

            return Json(project);
        }
        [HttpPost("/api/project/create")]
        [Produces("application/json")]
        public ActionResult Createproject([FromBody] ProjectFormModel formModel)
        {
            // write the given project to database
            int projectId = -1;
            using (EPortfolioDB database = new EPortfolioDB())
            {
                // create project with form model values
                Project project = new Project();
                formModel.Apply(project);

                // add new project to database
                database.Projects.Add(project);
                database.SaveChanges();
                projectId = project.ProjectId;
            }

            // respond with sucess message with inserted project id
            Object response = new { projectId = projectId };
            return Json(response);
        }
        // route to update project for project id and project form model
        [HttpPost("/api/project/update/{id}")]
        public ActionResult UpdateProject(
                int id, [FromBody] ProjectFormModel formModel)
        {
            using (EPortfolioDB database = new EPortfolioDB())
            {
                // Find the project specified by formModel
                Project project = database.Projects
                    .Where(s => s.ProjectId == id)
                    .Single();

                // perform Update using data in form model
                formModel.Apply(project);
                database.SaveChanges();
            }

            return Ok();
        }
        // route to delete project for project id
        [HttpPost("/api/project/delete/{id}")]
        public ActionResult DeleteProject(int id)
        {
            using (EPortfolioDB database = new EPortfolioDB())
            {
                // Find the project specified by formModel
                Project project = database.Projects
                    .Where(s => s.ProjectId == id)
                    .Single();

                // remove the Project from db
                database.Projects.Remove(project);
                database.SaveChanges();
            }

            return Ok();
        }
    }
}


