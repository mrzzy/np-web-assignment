using System;
using System.Collections.Generic;

namespace folio.Models
{
    public partial class Lecturer
    {
        public Lecturer()
        {
            Students = new HashSet<Student>();
            Suggestions = new HashSet<Suggestion>();
        }

        public int LecturerId { get; set; }
        public string Name { get; set; }
        public string EmailAddr { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Suggestion> Suggestions { get; set; }
    }
}
