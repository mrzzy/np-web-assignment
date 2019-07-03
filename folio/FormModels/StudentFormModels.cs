using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using folio.Models;

namespace folio.FormModels
{   
    // form model for creating students
    public class StudentCreateFormModel
    {
        [Required] 
        [DataType.Text]
        [MinLength(1)]
        public string Name { get; set; }

        [Required] 
        [MinLength(1)]
        [DataType.Text]
        public string Course { get; set; }

        [DataType.ImageUrl]
        public string Photo { get; set; }

        [DataType.Text]
        public string Description { get; set; }

        [DataType.Text]
        public string Achievement { get; set; }

        [DataType.Url]
        public string ExternalLink { get; set; }

        [Required] 
        [DataType.EmailAddr]
        public string EmailAddr { get; set; }

        [Required] 
        [DataType.Password]
        [MinLength(8, ErrorMessage="Passwords must be longer than 8 characters")]
        public string Password { get; set; }

        [Required] 
        public int MentorId { get; set; }
        
        // Create a new student from the data in this form model
        public Student Create()
        {
            Student student = new Student();
            student.Name = this.Name;
            student.Course = this.Course;
            student.Photo = this.Photo;
            student.Description = this.Description;
            student.Achievement = this.Achievement;
            student.ExternalLink = this.ExternalLink;
            student.EmailAddr = this.EmailAddr;
            student.Password = this.Password;
            student.MentorId = this.MentorId;
            
            return student;
        }
    }

    // form model for updating students
    public class StudentUpdateFormModel
    {
        [DataType.Text]
        [MinLength(1)]
        public string Name { get; set; }

        [MinLength(1)]
        [DataType.Text]
        public string Course { get; set; }

        [DataType.ImageUrl]
        public string Photo { get; set; }

        [DataType.Text]
        public string Description { get; set; }

        [DataType.Text]
        public string Achievement { get; set; }

        [DataType.Url]
        public string ExternalLink { get; set; }

        [DataType.EmailAddr]
        public string EmailAddr { get; set; }

        [DataType.Password]
        [MinLength(8, ErrorMessage="Passwords must be longer than 8 characters")]
        public string Password { get; set; }

        public int MentorId { get; set; }
        
        // Apply the properties in the student form model 
        public void Apply(Student student)
        {
            if(this.Name != null) student.Name = this.Name;
            if(this.Course != null) student.Course = this.Course;
            if(this.Photo != null) student.Photo = this.Photo;
            if(this.Description != null) student.Description = this.Description;
            if(this.Achievement != null) student.Achievement = this.Achievement;
            if(this.ExternalLink != null) student.ExternalLink = this.ExternalLink;
            if(this.Password != null) student.Password = this.Password;
            if(this.MentorId != null) student.MentorId = this.MentorId;
        }
    }
}
