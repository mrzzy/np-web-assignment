using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace folio.Models
{
    public partial class Suggestion
    {
        public int SuggestionId { get; set; }
        [Required]
        public int LecturerId { get; set; }
        [Required]
        public int StudentId { get; set; }
        [Required]
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual Lecturer Lecturer { get; set; }
        public virtual Student Student { get; set; }
    }
}
