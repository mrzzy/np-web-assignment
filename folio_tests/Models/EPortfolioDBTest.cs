using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;

using folio.Models;

namespace folio.Tests.Models
{
    public class EPortfolioDBTest
    {
        /* Intergration Tests - requires database to be present  */
        /* Project model */
        // Test insertion the project model into the database
        [Fact]
        public void InsertProject()
        {
            // Perform insertion of the project object
            int projectId = -1;
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
                Project project = database.Projects.Where(
                        (p) => p.ProjectId == projectId).First();
                Assert.True(ProjectTest.CheckSampleProject(project),
                        "Project obtained from database inconsistent with project" +
                        " inserted into database");
            }
        }
    }
}
