using System;
using System.Collections.Generic;
using Xunit;

using folio.Models;

namespace folio.Tests.Models
{
    public class ProjectTest
    {
        /* Unit Tests */
        [Fact]
        public void CheckProjectConstruction()
        {
            Project sampleProject =  this.GetSampleProject();
            Assert.True(this.CheckSampleProject(sampleProject), 
                    "Sample Project provided not consistent with original sample " +
                    "Project");
        }

        /* Utilties */
        // Create a new sample project model object and return it 
        public Project GetSampleProject()
        {
            Project sampleProject = new Project
            {
                Title = "Time Traveling with Deep Learning",
                Description = "In this project, We attempt to use deep learning" +
                          " to bend the fabric of space and time and achieve time travel.",
                ProjectPoster = "https://www.imgworlds.com/wp-content/uploads/2015/12/18-CONTACTUS-HEADER.jpg",
                ProjectUrl = "https://github.com/joeltio/sstannouncer"
            };
            
            return sampleProject;
        }
    
        // check the consistency of the project model created by GetSampleProject()
        // Returns true if the consistency is validated, false otherwise
        public bool CheckSampleProject(Project project)
        {
            if(project == null) return false;
            else if(project.Title != "Time Traveling with Deep Learning") 
                return false;
            else if(project.Description != "In this project, We attempt to use deep learning" +
                    " to bend the fabric of space and time and achieve time travel.") 
                return false;
            else if(project.ProjectPoster != 
                    "https://www.imgworlds.com/wp-content/uploads/2015/12/18-CONTACTUS-HEADER.jpg") 
                return false;
            else if(project.ProjectUrl != "https://github.com/joeltio/sstannouncer") 
                return false;
            
            return true;
        }
    }
}
