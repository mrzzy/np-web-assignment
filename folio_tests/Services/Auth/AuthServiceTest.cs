/*
 * NP Web Assignment
 * Authentication Service Tests
*/

using Xunit;
using System;
using DotNetEnv;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

using folio.Models;
using folio.FormModels;
using folio.Services.Auth;

namespace folio.Tests.Services
{
    public class AuthServiceTest
    {
        /* Intergration Tests */
        // test login with loginCredentials
        // NOTE: this test depends on existing data in the database as defined
        // in db setup SQL
        [Fact]
        public void TestWithLoginWithCredentials()
        {
            DotNetEnv.Env.Load();
            
            // attempt login with wrong credentials
            bool hasException = false;
            try
            {
                AuthService.Login(new LoginFormModel 
                {
                    EmailAddr = "s1234112@ap.edu.sg",
                    Password = "superman",
                    UserRole = "Student"
                });
            }
            catch
            {
                hasException = true;
            }
            Assert.True(hasException);
        
            // perform login 
            string token = AuthService.Login(new LoginFormModel
            {
                EmailAddr = "s1234112@ap.edu.sg",
                Password = "p@55Student",
                UserRole = "Student"
            });
            
            Assert.True(!String.IsNullOrWhiteSpace(token));
        }
        
        // test extraction of Session from HttpContext
        // NOTE: this test depends on existing data in the database as defined
        // in db setup SQL
        [Fact]
        public void TestExtractSession()
        {
            string token = AuthService.Login(new LoginFormModel
            {
                EmailAddr = "s1234112@ap.edu.sg",
                Password = "p@55Student",
                UserRole = "Student"
            });
            
            // create test http context with tokenk
            HttpContext context = new DefaultHttpContext();
            context.Request.Headers.Add("Authorization", "Bearer " + token);
            
            Session session = AuthService.ExtractSession(context);
            Assert.Equal( "s1234112@ap.edu.sg",session.EmailAddr);
            Assert.Equal("Student", session.MetaData["UserRole"]);
        }
    }
}
