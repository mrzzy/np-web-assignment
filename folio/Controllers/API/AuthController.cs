/*
 * Web Assignment 
 * Folio API
 * SkillSet API controller 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc.Rendering;

using folio.Models;
using folio.FormModels;
using folio.Services.Auth;

namespace folio.API.Controllers
{
    // controller for the /api/auth/ route
    public class AuthController: Controller
    {
        /* controller routes */
        // route to perform login given login credientials in  LoginFormModel
        // creates a temporary session token tha is be used authenticate
        // when making further api calls by setting the token as 
        // Bearer token in Authorization header of the reuqest.
        [HttpPost("/api/auth/login")]
        [Produces("application/json")]
        public ActionResult Login([FromBody] LoginFormModel loginCredentials)
        {
            // try to perform login
            string token = null;
            try { token = AuthService.Login(loginCredentials); }
            catch { return Unauthorized(); }
            
            // respond with temporary session token 
            Object response = new { SessionToken = token };
            return Json(response);
        }
        
        // route to check if the user is currently in a valid given 
        // Authorization bearer the SessionToken provided by Login()
        [HttpGet("/api/auth/check")]
        public ActionResult Check()
        {
            // check by trying load session from token
            try{ Session session = AuthService.ExtractSession(HttpContext); }
            catch { return Unauthorized(); }

            // authenticated
            return Ok();
        }

        // route to obtain infomation about the authorization bearer
        // token for the server
        [HttpGet("/api/auth/info")]
        public ActionResult GetTokenInfo()
        {
            // check by trying load session from token
            Session session = null;
            try{ session = AuthService.ExtractSession(HttpContext); }
            catch { return Unauthorized(); }

            // find user that matches the session's emailAddr
            HashSet<UserInfo> matchingUserInfos = new HashSet<UserInfo>();
            using(EPortfolioDB database = new EPortfolioDB())
            {
            
                if(session.MetaData["UserRole"] == "Lecturer")
                {
                    matchingUserInfos.UnionWith( database.Lecturers
                            .Where(l => l.EmailAddr == session.EmailAddr)
                            .Select(l => new UserInfo(l)));
                }
                else if(session.MetaData["UserRole"] == "Student")
                {
                    matchingUserInfos.UnionWith( database.Students
                            .Where(s => s.EmailAddr == session.EmailAddr)
                            .Select(s => new UserInfo(s)));
                }
            }

            // return matching user info as JSON
            if(matchingUserInfos.Count() <= 0) return Unauthorized();
            else return Json(matchingUserInfos.Single());
        }
    }
}
