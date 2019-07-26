using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using folio.Models;
using System.ComponentModel.DataAnnotations;

namespace folio.FormModels
{
    public class ProjectFormModel
    {

        [MinLength(1)]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Project name is required")]
        public string Title { get; set; }

        [DataType(DataType.Text)]
        public string Description { get; set; }

        [DataType(DataType.ImageUrl)]
        public string ProjectPoster { get; set; }

        [DataType(DataType.Url)]
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


