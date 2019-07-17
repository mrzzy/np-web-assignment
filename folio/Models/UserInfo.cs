/*
 * Web Assignment 
 * Folio API
 * User Info Model
*/

namespace folio.Models
{  
    // user info model defines properties common to all users
    public class UserInfo
    {    
        public int id;
        public string EmailAddr;
        public string UserRole;
        public string Name;

        // construct a userinfo given a lecturer model
        public UserInfo(Lecturer lecturer)
        {
            this.id = lecturer.LecturerId;
            this.EmailAddr = lecturer.EmailAddr;
            this.UserRole = "Lecturer";
            this.Name = lecturer.Name;
        }
        
        // construct a userinfo given a student model
        public UserInfo(Student student)
        {
            this.id = student.StudentId;
            this.EmailAddr = student.EmailAddr;
            this.UserRole = "Student";
            this.Name = student.Name;
        }
    }
}
