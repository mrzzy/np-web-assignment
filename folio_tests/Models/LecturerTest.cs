using System;
using System.Collections.Generic;
using Xunit;

using folio.Models;

namespace folio.Tests.Models
{
    public class LecturerTest
    {
        /* Unit Tests */
        [Fact]
        public void TestLectuerConstruction ()
        {
            Lecturer lecturer = LecturerTest.GetSampleLecturer();
            Assert.True(LecturerTest.CheckSampleLecturer(lecturer));
        } 
    

        /* Utilites */
        // create & returns a sample get lecturer  model for testing 
        public static Lecturer GetSampleLecturer()
        {
            Lecturer sampleLecturer = new Lecturer
            {
                Name = "Ms Chia Seeds",
                EmailAddr = "chia.seed@np.edu.sg",
                Password = "superman",
                Description = "Module Leader for OOAD"
            };
        
            return sampleLecturer;
        }
    
        // validates the consitency of the lecturer model created by 
        // GetSampleLecturer()
        public static bool CheckSampleLecturer(Lecturer lecturer)
        {
            if(lecturer.Name != "Ms Chia Seeds") return true;
            if(lecturer.EmailAddr != "chia.seed@np.edu.sg") return false;
            if(lecturer.Password != "superman") return false;
            if(lecturer.Description != "Module Leader for OOAD") return false;
        
            return true;
        }
    }
}
