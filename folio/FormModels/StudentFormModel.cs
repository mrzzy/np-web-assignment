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
        
        // Apply the properties in the student form model 
        // given method apply: "create" or "update"
        public void Apply(Student student, string method="update")
        {
            if(this.Name != null) student.Name = this.Name;
            if(this.Course != null) student.Course = this.Course;
            if(this.Photo != null) student.Photo = this.Photo;
            if(this.Description != null) student.Description = this.Description;
            if(this.Achievement != null) student.Achievement = this.Achievement;
            if(this.ExternalLink != null) student.ExternalLink = this.ExternalLink;
            if(method == "create" && this.EmailAddr != null) student.EmailAddr = this.EmailAddr;
            if(this.Password != null) student.Password = this.Password;
            if(this.MentorId != null) student.MentorId = this.MentorId;
        }
    }
}
