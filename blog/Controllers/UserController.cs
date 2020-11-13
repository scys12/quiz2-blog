using blog.Models.Repository;
using blog.Models;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Security.Cryptography;
using System.Web.Security;

namespace blog.Controllers
{
    public class UserController : Controller
    {
        // Registration Action
        private UserRepository userRepository = new UserRepository();

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Registration()
        {
            if (HttpContext.User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Registration(RegisterViewModel registerModel)
        {
            string message = "";
            string status = "";
            if (ModelState.IsValid)
            {
                var isEmailExist = userRepository.CheckEmailExist(registerModel.Email);
                if (!isEmailExist)
                {
                    registerModel.Password = GetMD5(registerModel.Password);
                    userRepository.RegisterUser(registerModel);
                    message = "Account registered successfully";
                    status = "success";
                }
                else
                {
                    message = "Your email have been registered";
                    status = "danger";
                }
            }
            ModelState.Clear();
            ViewBag.Message = message;
            ViewBag.Status = status;
            return View();
        }

        // Login Action
        [HttpGet]
        public ActionResult Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel loginModel, string ReturnUrl="")
        {
            string message = "";
            string status = "";
            if (ModelState.IsValid)
            {
                loginModel.Password = GetMD5(loginModel.Password);
                var userData = userRepository.Login(loginModel);
                if (userData != null)
                {
                    Session["FullName"] = userData.FullName;
                    Session["Email"] = userData.Email;
                    Session["UserID"] = userData.UserID;

                    int timeout = 1008;
                    var ticket = new FormsAuthenticationTicket(loginModel.Email, true, timeout);
                    var encrypted = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                    cookie.Expires = DateTime.Now.AddMinutes(timeout);
                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                 
                    }else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    message = "Email/Password Wrong";
                    status = "danger";
                }
            }
            ViewBag.Message = message;
            ViewBag.Status = status;
            return View();
        }

        public string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

        // Log Out 
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }

    }
}