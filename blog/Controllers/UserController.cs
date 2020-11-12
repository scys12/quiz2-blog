using RegistrationAndLogin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace RegistrationAndLogin.Controllers
{
    public class UserController : Controller
    {
        // Registration Action
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        // Login Action
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // Log Out 
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }

    }
}