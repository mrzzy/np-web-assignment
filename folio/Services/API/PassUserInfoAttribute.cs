/*
 * PassUserInfo Attribute
 * NP Web Assignment
*/

using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using folio.Models;
                
namespace folio.Services.API
{
    /* defines attribute that injects UserInfo as view data entry
     * if no user UserInfo is available will inject UserInfo as null
    */
    public class PassUserInfoAttribute: ActionFilterAttribute
    {
        /* constructors */
        // construct a PassUserInfoAttribute to pass user info as an attribute
        public PassUserInfoAttribute() { }

        /* ActionFilterAttribute overrides */
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // pull user info form api and pass as view data
            APIClient api = new APIClient(context.HttpContext);
            (context.Controller as Controller).ViewData["UserInfo"] = api.GetUserInfo();
        }
    }
}
