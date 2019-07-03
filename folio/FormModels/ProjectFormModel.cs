using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using folio.Models;

namespace folio.FormModels
{
    public class ProjectFormModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ProjectPoster { get; set; }
        public string ProjectURL { get; set; }

        // Apply the the values of the properties of the Project form model
        // to the given Project model 
        public void Apply(Project project)
        {
            project.Title = this.Title;
            project.Description = this.Description;
            project.ProjectPoster = this.ProjectPoster;
            project.ProjectUrl = this.ProjectURL;

        }
    }
}


