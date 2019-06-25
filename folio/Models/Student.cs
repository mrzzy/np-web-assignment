using System;
using System.Collections.Generic;

namespace folio.Models
{
    public partial class Student
    {
        public Student()
        {
            Suggestion = new HashSet<Suggestion>();
            this.StudentSkillSets = new HashSet<StudentSkillSet>();
        }

        public int StudentId { get; set; }
        public string Name { get; set; }
        public string Course { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
        public string Achievement { get; set; }
        public string ExternalLink { get; set; }
        public string EmailAddr { get; set; }
        public string Password { get; set; }
        public int MentorId { get; set; }

        // Foreign model relationships
        public virtual Lecturer Mentor { get; set; }
        public virtual ICollection<Suggestion> Suggestion { get; set; }
        public virtual ICollection<StudentSkillSet> StudentSkillSets { get; set; }
    }
}
