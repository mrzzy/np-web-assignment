using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Xunit;
using DotNetEnv;

using folio.Models;

namespace folio.Tests.Models
{
    public class EPortfolioDBTest
    {
        /* DB Intergration Tests - requires database to be present  */
        // Test insertion the model into the database
        [Fact]
        public void TestInsertModel()
        {
            // load environment variables from .env
            DotNetEnv.Env.Load();
            
            // Perform insertion of the project model
            int projectId = -1; // -1 -> null value
            using (EPortfolioDB database = new EPortfolioDB())
            {
                Project project = ProjectTest.GetSampleProject();
                database.Projects.Add(project);
                database.SaveChanges();
                projectId = project.ProjectId;
            }

            // Check for presence of record in the database
            using (EPortfolioDB database = new EPortfolioDB())
            {
                Project obtainProject = database.Projects.Where(
                        (p) => p.ProjectId == projectId).First();
                Assert.True(ProjectTest.CheckSampleProject(obtainProject),
                        "Project obtained from database inconsistent with project" +
                        " inserted into database");
                
                // cleanup
                database.Projects.Remove(obtainProject);
                database.SaveChanges();
            }
        }
        
    
        [Fact]
        // test updating models in the database
        public void TestUpdateModel()
        {
            // load environment variables from .env
            DotNetEnv.Env.Load();
            
            int projectId = 0;
            using (EPortfolioDB database = new EPortfolioDB())
            {
                // insert test project into db
                Project project = ProjectTest.GetSampleProject();
                database.Projects.Add(project);
                database.SaveChanges();
                projectId = project.ProjectId;

                // check that the project has not yet changed
                Assert.True(ProjectTest.CheckSampleProject(project));
                
                // Perform update on model
                project.Title = "Deep Learning Time Travel";
                database.SaveChanges();
            }
            
            // check that the project has actually been updated
            using (EPortfolioDB database = new EPortfolioDB())
            {
                Project obtainProject = database.Projects.Where(
                        (p) => p.ProjectId == projectId).First();
                Assert.False(ProjectTest.CheckSampleProject(obtainProject),
                        "Project update changes has not propogated to database");
                Assert.Equal(obtainProject.Title, "Deep Learning Time Travel");
            
                // cleanup
                database.Projects.Remove(obtainProject);
                database.SaveChanges();
            }
        }

        // test deletion the model into the database
        [Fact]
        public void TestDeleteModel()
        {
            int projectId = -1; // -1 -> null value
            using (EPortfolioDB database = new EPortfolioDB())
            {
                // Perform insertion of the test project
                Project project = ProjectTest.GetSampleProject();
                database.Projects.Add(project);
                database.SaveChanges();
                projectId = project.ProjectId;
                
                // delete the test project
                database.Projects.Remove(project);
                database.SaveChanges();
            }

            // Check for presence of record in the database
            using (EPortfolioDB database = new EPortfolioDB())
            {
                int nMatches = database.Projects
                    .Where(p => p.ProjectId == projectId).Count();
                Assert.Equal(nMatches, 0);
            }
        }
        
        // test querying ability of the model, including traversing
        // foreign model relations.
        // NOTE: relies on prepopulated data defined in
        // db/Student_EPortfolio_Db_SetUp_Script.sql
        [Fact]
        public void TestQueryModel()
        {
            // load environment variables from .env
            DotNetEnv.Env.Load();

            // query for  predefined data
            using (EPortfolioDB database = new EPortfolioDB())
            {
                Student student = database.Students
                    .Where(s => s.Name == "Amy Ng")
                    .Include(s => s.ProjectMembers)
                        .ThenInclude(pm => pm.Project)
                    .Single();
                
                Assert.True(student.StudentId == 2,
                        "Query returned wrong student model");

                Project obtainProject = student.ProjectMembers.First().Project;
                Assert.True(obtainProject.ProjectId == 2,
                        "Traversing foreign relations gave wrong project");
            }
        }
        
        // Test the insertion, query of a complex model (model with foreign model
        // relationships
        [Fact]
        public void TestComplexModel()
        {
            // load environment variables from .env
            DotNetEnv.Env.Load();

            /* Insert a complex model with foreign model relationships 
             * <- means depends foreign model
             * Lecturer <- Student <- ProjectMembers -> Project 
            */
            int lecturerId = -1;
            using (EPortfolioDB database = new EPortfolioDB())
            {
                // Lecturer model
                Lecturer lecturer = LecturerTest.GetSampleLecturer();
                database.Lecturers.Add(lecturer);
            
                // Student model
                Student student = StudentTest.GetSampleStudent();
                student.Mentor = lecturer;
                database.Students.Add(student);
        
                // Project model
                Project project = ProjectTest.GetSampleProject();
                database.Projects.Add(project);
            
                // ProjectMember model
                ProjectMember projectMember = new ProjectMember
                {
                    Member = student,
                    Project = project
                };
                database.ProjectMembers.Add(projectMember);

                database.SaveChanges();
                lecturerId = lecturer.LecturerId;
            }
        
            // query complex model
            using (EPortfolioDB database = new EPortfolioDB())
            {
                Lecturer lecturer = database.Lecturers
                    .Where(l => l.LecturerId == lecturerId)
                    .Include(l => l.Students)
                        .ThenInclude(s => s.ProjectMembers)
                            .ThenInclude(pm => pm.Project)
                    .Single();

                Assert.True(LecturerTest.CheckSampleLecturer(lecturer),
                    "lecturer obtained from DB inconsistent with one inserted " +
                    "into the db");
                
                Student student = lecturer.Students.First();
                Assert.True(StudentTest.CheckSampleStudent(student),
                    "student obtained from DB inconsistent with one inserted " +
                    "into the db");
                
                
                ProjectMember projectMember = lecturer.Students.First()
                                                .ProjectMembers.First();
                Project project = projectMember.Project;
                Assert.True(ProjectTest.CheckSampleProject(project),
                    "project obtained from DB inconsistent with one inserted " +
                    "into the db");

                // cleanup
                database.ProjectMembers.Remove(projectMember);
                database.Projects.Remove(project);
                database.Students.Remove(student);
                database.Lecturers.Remove(lecturer);
                database.SaveChanges();
            }
        }
    }
}
