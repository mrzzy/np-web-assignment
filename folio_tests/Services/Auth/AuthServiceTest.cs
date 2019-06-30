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

public class AuthServiceTest
{
    /* Intergration Tests */
    // NOTE: this test depends on existing data in the database as defined
    // in db setup SQL
    public void AuthServiceCheckCredentialsTest()
    {
        // attempt to authenticate with wrong credentials
        Assert.False(AuthService.CheckCredentials(new LoginFormModel 
        {
            EmailAddr = "s1234112@ap.edu.sg",
            Password = "superman"
        }));
    
        // attempt to authenticate with correct credentials
        Assert.True(AuthService.CheckCredentials(new LoginFormModel
        {
            EmailAddr = "s1234112@ap.edu.sg",
            Password = "p@55Student"
        }));
    }
}
