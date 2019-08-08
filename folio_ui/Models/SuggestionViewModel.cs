using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using folio.Models;

namespace folio_ui.Models
{
    public class SuggestionViewModel
    {
        public List<Student> studentList { get; set; }

        public List<Suggestion> suggestionList { get; set; }
    }
}
