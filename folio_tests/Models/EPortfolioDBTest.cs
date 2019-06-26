using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Xunit;

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
                int nMatches = database.Projects.Where(p => p.ProjectId == projectId).Count();
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
            // query 
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
    }
}
