using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace folio.Models
{
    public class MenteesViewModel
    {
        public List<Lecturer> lecturerList { get; set; }
        public List<Suggestion> suggestionList { get; set; }
        public List<Student> studentList { get; set; }
    }
}
