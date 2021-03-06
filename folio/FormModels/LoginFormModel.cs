/*
 * NP Web Assignment
 * Login Form Model
*/

using folio.Models;

namespace folio.FormModels
{
    // model defines the properties required to login
    public class LoginFormModel
    {
        public string EmailAddr  { get; set; }
        public string Password  { get; set; }
        public string UserRole { get; set; } = null;

        /* public utility constructors */
        public LoginFormModel() {}

        // construct a login form model from given student
        public LoginFormModel(Student student)
        {
            this.EmailAddr = student.EmailAddr;
            this.Password = student.Password;
            this.UserRole = "Student";
        }
        
        // construct a login form model from given lecturer
        public LoginFormModel(Lecturer lecturer)
        {
            this.EmailAddr = lecturer.EmailAddr;
            this.Password = lecturer.Password;
            this.UserRole = "Lecturer";
        }

        // define instance equality
        public override bool Equals(object obj)
        {
            // check for null and type missmatch
            if(obj == null) return false;
            if (this.GetType() != obj.GetType()) return false;
            
            // check object properties
            LoginFormModel other = (LoginFormModel)obj;
            if(this.EmailAddr != other.EmailAddr ||
                this.Password != other.Password) return false;

            return true;
        }
    
        // compute & return hash code from object state 
        public override int GetHashCode()
        {
            int hashCode = 13;
            hashCode = (hashCode * 7) ^ this.EmailAddr.GetHashCode();
            hashCode = (hashCode * 7) ^ this.Password.GetHashCode();
            
            return hashCode;
        }
    }
}
