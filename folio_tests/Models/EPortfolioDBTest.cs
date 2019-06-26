using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;

using folio.Models;

namespace folio.Tests.Models
{
    public class EPortfolioDBTest
    {
        /* DB Intergration Tests - requires database to be present  */
        /* Project model */
        // Test insertion the project model into the database
        [Fact]
        public void TestInsertModel()
        {
            // Perform insertion of the project object
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
        public void TestUpdateModel()
        {
            
            // Update object in database
            int projectId = 0;
            using (EPortfolioDB database = new EPortfolioDB())
            {
                // Test Projects
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
            
            // check that the object has actually been updated
            using (EPortfolioDB database = new EPortfolioDB())
            {
                Project obtainProject = database.Projects.Where(
                        (p) => p.ProjectId == projectId).First();
                Assert.False(ProjectTest.CheckSampleProject(obtainProject),
                        "Project update changes has not propogated to database");
                Assert.Equal(obtainProject.Title, "Deep Learning Time Travel");
                database.SaveChanges();
            }
        }
    }
}
