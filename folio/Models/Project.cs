using System;
using System.Collections.Generic;

namespace folio.Models
{
    public partial class Project
    {
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ProjectPoster { get; set; }
        public string ProjectUrl { get; set; }
    }
}
