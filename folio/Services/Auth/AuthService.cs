/*
 * NP Web Assignment
 * Authentication Service
*/

using System;
using System.Linq;
using System.Collections.Generic;
using folio.Models;
using folio.FormModels;

namespace folio.Services.Auth
{
    // Authentication exception
    public class AuthException: Exception 
    {
        public AuthException(string message): base(message) {}
    }

    public class AuthService
    { 
        // Authenticate the user with the given login form model and password.
        // Throw AuthException on authentication failure 
        // Returns a JWT token that the user can use to authenticate temporary
        // (ie session)
        public string Login(LoginFormModel loginCredentials)
        { 
            AuthService.CheckCredentials(loginCredentials);
        
            // TODO: Prepare JWT token
            return "";
        }
        
    
        // Check the given login credentials.
        // Returns true on success,false on failure
        public static bool CheckCredentials(LoginFormModel loginCredentials)
        {
            HashSet<LoginFormModel> dbLoginCreds = new HashSet<LoginFormModel>();
            using(EPortfolioDB database = new EPortfolioDB())
            {
                // collect all matching users in the database
                dbLoginCreds.UnionWith( database.Students
                        .Where(s => s.EmailAddr == loginCredentials.EmailAddr)
                        .Select(s => new LoginFormModel(s)));
                dbLoginCreds.UnionWith( database.Lecturers
                        .Where(l => l.EmailAddr == loginCredentials.EmailAddr)
                        .Select(l => new LoginFormModel(l)));
            }
            
            // verify  login credentials
            // check if any users actualy matched
            if(dbLoginCreds.Count() <= 0) return false;
            // check if credentials match
            else if(dbLoginCreds.Single() != loginCredentials) return false;

            return true;
        }
    }
}
