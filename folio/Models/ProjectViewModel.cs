using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace folio.Models
{
    public class ProjectViewModel
    {
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string ProjectUrl { get; set; }
        public string Password { get; set; }

        public string Description { get; set; }

        public string ProjectPoster { get; set; }
        public IFormFile FileToUpload { get; set; }
    }
}
