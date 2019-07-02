using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using folio.Models;

namespace folio.FormModels
{
    public class SuggestionFormModel
    {
        public int lecturerId { get; set; }
        public int studentId { get; set; }
        public string Description { get; set; }

        public DateTime dateCreated = DateTime.Now;

        public void Apply(Suggestion s)
        {
            s.LecturerId = this.lecturerId;
            s.StudentId = this.studentId;
            s.Description = this.Description;
            s.DateCreated = this.dateCreated;
            
        }
    }
}
