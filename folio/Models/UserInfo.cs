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
        public string EmailAddr;
        public string UserRole;
        public string Name;

        // construct a userinfo given a lecturer model
        public UserInfo(Lecturer lecturer)
        {
            this.EmailAddr = lecturer.EmailAddr;
            this.UserRole = "Lecturer";
            this.Name = lecturer.Name;
        }
        
        // construct a userinfo given a student model
        public UserInfo(Student student)
        {
            this.EmailAddr = student.EmailAddr;
            this.UserRole = "Student";
            this.Name = student.Name;
        }
    }
}
