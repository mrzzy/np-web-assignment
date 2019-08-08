/*
 * Authentication Attribute
 * NP Web Assignment
*/

using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Security.Authentication;


namespace folio.Services.Auth
{
    /* Defines Action Attribute performs Authentication Checks before the action */
    public class AuthenticateAttribute : ActionFilterAttribute
    {
        private string UserRole { get; set; } = null;
    
        /* constructors */ 
        // construct an authenticate attribute that checks that the user is 
        // authenticated & in a valid session
        public AuthenticateAttribute() { }

        // construct an authenticate attribute that checks that the user is 
        // both authenticated & in a valid session and check that user has the 
        // given user role
        public AuthenticateAttribute(string userRole)
        {
            this.UserRole = userRole;
        }
        
        /* ActionFilterAttribute overrides */
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // check whether the user is authenticated in valid session
            Session session = null;
            try { session = AuthService.ExtractSession(context.HttpContext); }
            catch 
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            
            // check if the use has the specified role (if specified at all)
            if(this.UserRole != null && this.UserRole != session.MetaData["UserRole"])
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    
    }
}
