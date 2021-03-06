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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace folio.Controllers.API
{
    public class ProjectController : Controller
    {
        private ActionResult ProjectTitleConflict
        {
            get
            {
                return Conflict(new Dictionary<string, string>
                {
                    { "Title", "Name of Title conflicts with an existing project" }
                });
            }
        }

        /* Controller Routes */

        // route to query projects, with optional specification in url params:
        // name - filter the url parameter by exact match name
        // limit - limit results returned to the given no.
        // responds to request with the ids of all matching skillsets
        [HttpGet("/api/projects")]
        [Produces("application/json")]
        public ActionResult Query([FromQuery] string name, [FromQuery] int? limit, int? student)
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
                if (student != null)
                {
                    // filter by project id 
                    matchingProjects = matchingProjects
                        .Include(s => s.ProjectMembers)
                        .Where(s => s.ProjectMembers
                            .Any(pm => pm.StudentId == student));
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
        [HttpGet("/api/projects/details")]
        [Produces("application/json")]
        public ActionResult AllProjects(int id)
        {
            List<Project> projectList = new List<Project>();
            using (EPortfolioDB db = new EPortfolioDB())
            {
                projectList = db.Projects.ToList();
                db.SaveChanges();
            }
            return Json(projectList);
        }
        [HttpPost("/api/project/create")]
        [Authenticate("Student")]
        [Produces("application/json")]
        public ActionResult Createproject([FromBody] ProjectFormModel formModel)
        {

            // check if authenticated
            if (!ModelState.IsValid)
            { return BadRequest(ModelState); }

            // write the given project to database
            int projectId = -1;
            using (EPortfolioDB database = new EPortfolioDB())
            {

                // check if project name does not conflict with existing project
                if (database.Projects
                    .Where(s => s.Title == formModel.Title)
                    .Count() >= 1)
                { return ProjectTitleConflict; }

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
        [Authenticate("Student")]
        public ActionResult UpdateProject(
                int id, [FromBody] ProjectFormModel formModel)
        {
            if (!ModelState.IsValid)
            { return BadRequest(ModelState); }

            using (EPortfolioDB database = new EPortfolioDB())
            {
                // check if project name does not conflict with existing project
                if (database.Projects
                    .Where(s => s.Title == formModel.Title)
                    .Count() >= 1)
                { return ProjectTitleConflict; }

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
        [Authenticate("Student")]
        public ActionResult DeleteProject(int id)
        {
     
            using (EPortfolioDB database = new EPortfolioDB())
            {
                // Find the project specified by formModel
                Project project = database.Projects
                    .Where(s => s.ProjectId == id)
                    .Include(s => s.ProjectMembers)
                    .FirstOrDefault();
                if (project == null)
                { return NotFound(); }

                // cascade delete any ProjectMember assignments
                IQueryable<ProjectMember> assignments = database.ProjectMembers
                    .Where(s => s.ProjectId == id);
                database.ProjectMembers.RemoveRange(assignments);

                // remove the Project from db
                database.Projects.Remove(project);
                database.SaveChanges();
            }

            return Ok();
        }
        [HttpPost("/api/project/assign/{id}")]
        [Authenticate("Student")]
        public ActionResult AssignProjectSet(int id, [FromQuery] int student)
        {
            using (EPortfolioDB database = new EPortfolioDB())
            {

                // find if student is already assigned into project
                IQueryable<ProjectMember> matchingAssignments = database
                    .ProjectMembers
                        .Where(s => s.ProjectId == id)
                        .Where(s => s.StudentId == student);
                if (matchingAssignments.Count() >= 1)
                    return Ok(); // already assigned: nothing to do

                // find if the project leader is already assigned into project.
                ProjectMember roleleader = database
                    .ProjectMembers
                        .Where(s => s.ProjectId == id)
                        .Where(s => s.Role == "Leader").FirstOrDefault();

                // determine if project already has leader
                // if already has leader, assign as normal member
                // otherwise assign as project leader
                string role = "";
                if (roleleader == null)
                {
                    role = "Leader";
                            
                }
                else
                {
                    role = "Member";   
                }

                //Create and save the project member to database
                Project projectModel = database.Projects
                    .Where(s => s.ProjectId == id).Single();
                Student studentModel = database.Students
                    .Where(s => s.StudentId == student).Single();

                ProjectMember assignment = new ProjectMember
                {
                    Member = studentModel,
                    Project = projectModel,
                    Role = role
                };
                database.ProjectMembers.Add(assignment);
                database.SaveChanges();
            }

            return Ok();
        }
        [HttpPost("/api/project/remove/{id}")]
        [Authenticate("Student")]
        public ActionResult RemoveProject(int id, [FromQuery] int student)
        {
            using(EPortfolioDB database = new EPortfolioDB())
            {
                
                ProjectMember assignment  = database.ProjectMembers
                    .Where(s => s.ProjectId == id)
                    .Where(s => s.StudentId == student)
                    .Single();
                
               
                database.ProjectMembers.Remove(assignment);
                database.SaveChanges();
            }

            return Ok();
        }
      
        [HttpGet("/api/project/member/{id}")]
        [Produces("application/json")]
        [Authenticate("Student")]
        public ActionResult GetMember(int id)
        {
            Console.WriteLine("get id:", id.ToString());
            // Retrieve the lecturer for id
            List<ProjectMember> projectList = new List<ProjectMember>();
            using (EPortfolioDB database = new EPortfolioDB())
            {
                projectList = database.ProjectMembers
                    .Where(s => s.ProjectId == id).ToList();
                    
                database.SaveChanges();

            }

            if (projectList == null) return NotFound();

            return Json(projectList);
        }
      
    }
}


