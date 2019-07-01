/*
 * NP Web Assignment
 * Authentication Service Tests
*/

using Xunit;
using System;
using DotNetEnv;
using System.Linq;
using System.Collections.Generic;

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
                    Password = "superman"
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
                Password = "p@55Student"
            });
            
            Assert.True(!String.IsNullOrWhiteSpace(token));
        }
    }
}
