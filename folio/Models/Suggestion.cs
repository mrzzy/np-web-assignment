using System;
using System.Collections.Generic;

namespace folio.Models
{
    public partial class Suggestion
    {
        public int SuggestionId { get; set; }
        public int LecturerId { get; set; }
        public int StudentId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual Lecturer Lecturer { get; set; }
        public virtual Student Student { get; set; }
    }
}
