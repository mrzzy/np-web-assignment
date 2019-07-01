/*
 * NP Web Assignment
 * Authentication Service
*/

using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

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
        /* properties */
        private static byte[] GetSessionSecretKey()
        {
            // load secret key for signing sessions from enviroment
            string keyStr = Environment.GetEnvironmentVariable("JWT_SECRET");
            if(string.IsNullOrWhiteSpace(keyStr))
            {
                throw new ArgumentNullException("Required JWT_SECRET secret key" 
                        + " secret key for signing tokens envrionment variable"
                        + " not set");
            }
            byte[] secretKey = AuthService.ConvertToBytes(keyStr);

            return secretKey;
        }
        
        // Authenticate the user with the given login form model and password.
        // provided the given login
        // Returns a session token that can be used to temporary authenticate 
        // with the user
        // Throw AuthException on authentication failure 
        public static string Login(LoginFormModel loginCredentials)
        { 
            LoginFormModel dbLoginCreds = 
                AuthService.FindUser(loginCredentials.EmailAddr);
            if(dbLoginCreds == null || !dbLoginCreds.Equals(loginCredentials))
            {
                throw new AuthException("User not found or invalid credentials");
            }
        
            // create temporary session token for the user    
            Session session = new Session(loginCredentials.EmailAddr);
            string token = session.ToJWT(AuthService.GetSessionSecretKey());
        
            return token;
        }
    
        // Attempt Extract the session from the given http context
        // Returns the extracted session object.
        // Throws AuthException on session load failure
        public static Session ExtractSession(HttpContext context)
        {
            // extract JWT token from context & reconstruct session
            string token = context.Request.Headers["Authorization"];
            Session session = Session.FromJWT(token, 
                    AuthService.GetSessionSecretKey());
        
            // check if token references and actual user
            if(AuthService.FindUser(session.EmailAddr) == null)
            {
                throw new AuthException("Loaded Session references a non existent user");
            }

            return session;
        }
    
        /* private utilities */
        // convert the given hex dump into a list of bytes
        private static Byte[] ConvertToBytes(string hexDump)
        {
            // construct a list of pairs of hex digits to form bytes
            List<string> bytePairs = new List<string>();
            for(int i = 0; i < hexDump.Count(); i += 2)
            {
                bytePairs.Add(hexDump.Substring(i, 2));
            }
        
            // convert hex pairs to bytes array
            return bytePairs.Select(hexPair => Convert.ToByte(hexPair, 16))
                .ToArray();
        }
        
        // find the login credentials of then user with the given emailAddr
        // or null of if no such user exists
        private static LoginFormModel FindUser(string EmailAddr)
        {
            HashSet<LoginFormModel> dbLoginCreds = new HashSet<LoginFormModel>();
            using(EPortfolioDB database = new EPortfolioDB())
            {
                // collect all matching users in the database
                dbLoginCreds.UnionWith( database.Students
                        .Where(s => s.EmailAddr == EmailAddr)
                        .Select(s => new LoginFormModel(s)));
                dbLoginCreds.UnionWith( database.Lecturers
                        .Where(l => l.EmailAddr == EmailAddr)
                        .Select(l => new LoginFormModel(l)));
            }
        
            if(dbLoginCreds.Count() <= 0) return null;
            return dbLoginCreds.Single();
        }
    }
}
