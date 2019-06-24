using System;
using System.Collections.Generic;

namespace folio.Models
{
    public partial class Lecturer
    {
        public Lecturer()
        {
            Student = new HashSet<Student>();
            Suggestion = new HashSet<Suggestion>();
        }

        public int LecturerId { get; set; }
        public string Name { get; set; }
        public string EmailAddr { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Student> Student { get; set; }
        public virtual ICollection<Suggestion> Suggestion { get; set; }
    }
}
