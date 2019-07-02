/*
 * Web Assignment 
 * Folio API
 * SkillSet API controller 
*/

using System;
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
        // Authorization Bearin the SessionToken provided by Login()
        [HttpGet("/api/auth/check")]
        public ActionResult Check()
        {
            // check by trying load session from token
            try{ Session session = AuthService.ExtractSession(HttpContext); }
            catch { return Unauthorized(); }

            // authenticated
            return Ok();
        }
    }
}
