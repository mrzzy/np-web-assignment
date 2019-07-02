using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using folio.Models;

namespace folio.FormModels
{
    public class StudentFormModel
    {
        public string Name { get; set; }
        public string Course { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
        public string Achievement { get; set; }
        public string ExternalLink { get; set; }
        public string EmailAddr { get; set; }
        public string Password { get; set; }
        public int MentorId { get; set; }

        public void Apply(Student student)
        {
            student.Name = this.Name;
            student.Course = this.Course;
            student.Photo = this.Photo;
            student.Description = this.Description;
            student.Achievement = this.Achievement;
            student.ExternalLink = this.ExternalLink;
            student.EmailAddr = this.EmailAddr;
            student.Password = this.Password;
            student.MentorId = this.MentorId;
        }
    }
}
