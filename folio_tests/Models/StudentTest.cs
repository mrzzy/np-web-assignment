using System;
using System.Collections.Generic;
using Xunit;

using folio.Models;

namespace folio.Tests.Models
{
    public class StudentTest
    {
        /* Unit Tests */
        [Fact]
        public void TestStudentContruction ()
        {
            Student sampleStudent = StudentTest.GetSampleStudent();
            Assert.True(StudentTest.CheckSampleStudent(sampleStudent));
        } 
    
    
        /* Utilties */
        // create & returns a sample student model for testing 
        public static Student GetSampleStudent()
        {
            Student sampleStudent = new Student 
            {
                Name = "Joel Hwee",
                Course = "Infomation Technology",
                Photo = "https://i.kinja-img.com/gawker-media/image/upload/s--yUd5hQ3U--/c_fill,f_auto,fl_progressive,g_center,h_675,pg_1,q_80,w_1200/18r8i93r440epjpg.jpg",
                Description = "very smart person",
                Achievement = "PHD in Computer Science",
                ExternalLink = "https://www.linkedin.com/in/joel-tio/?originalSubdomain=sg",
                EmailAddr = "joel.hwee@np.edu.sg",
            };

            return sampleStudent;
        }

        // validates the consitency of the student model created by GetSampleStudent()
        public static bool CheckSampleStudent(Student student)
        {
            if(student.Name != "Joel Hwee") return false;
            if(student.Course != "Infomation Technology") return false;
            if(student.Photo != "https://i.kinja-img.com/gawker-media/image/upload/s--yUd5hQ3U--/c_fill,f_auto,fl_progressive,g_center,h_675,pg_1,q_80,w_1200/18r8i93r440epjpg.jpg") return false;
            if(student.Description != "very smart person") return false;
            if(student.Achievement != "PHD in Computer Science") return false;
            if(student.ExternalLink != "https://www.linkedin.com/in/joel-tio/?originalSubdomain=sg") return false;
            if(student.EmailAddr != "joel.hwee@np.edu.sg") return false;
        
            return true;
        }
    }
}
