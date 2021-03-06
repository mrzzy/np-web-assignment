/*
 * Folio UI
 * Authentication UI Controller
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace folio_ui.Controllers
{
    public class AuthController: Controller
    {
        // Login action renders login page to allow the user to perform a login
        public ActionResult Login()
        {
            return View();
        }
    
        // sign up action provides user with option to sign up as either a
        // student or lecturer
        public ActionResult SignUp()
        {
            return View();
        }
    }
}
