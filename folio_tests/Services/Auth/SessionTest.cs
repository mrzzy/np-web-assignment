/*
 * NP Web Assignment
 * Authentication Service Tests
*/

using Xunit;
using System;
using System.Linq;
using System.Collections.Generic;

using folio.Models;
using folio.FormModels;
using folio.Services.Auth;

namespace folio.Tests.Services
{
    public class SessionTest
    {  
        /* Unit Tests */
        [Fact]
        public void TestSessionConstructor()
        {
            Session session = new Session("joel@gmail.com", "p@ssw0rd");
            
            Assert.Equal("joel@gmail.com",session.EmailAddr);
            Assert.Equal(new Dictionary<string, string>(), session.MetaData);
        }
        
    
        [Fact]
        // test the conversion of a session to and from JWT
        public void TestToFromJWT()
        {
            Session originalSession = new Session("joel@gmail.com", "p@ssw0rd");
            originalSession.MetaData.Add("name", "Dr Joel");

            // convert to JWT token with hardcoded secret key
            byte[] secretKey = new byte[]{164,60,194,0,161,189,41,38,130,89,141,
                164,45,170,159,209,69,137,243,216,191,131,47,250,32,107,231,117,
                37,158,225,234};
            string token =  originalSession.ToJWT(secretKey);

            // convert from JWT using secret key
            Session recreatedSession = Session.FromJWT(token, secretKey);
            Assert.True(originalSession.Equals(recreatedSession));
        }
    }
}
