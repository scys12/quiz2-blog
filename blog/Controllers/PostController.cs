using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using blog.Models.POCO;

namespace blog.Controllers
{
    public class PostController : Controller
    {
        private string CS = ConfigurationManager.ConnectionStrings["blog"].ConnectionString;
        public ActionResult Index()
        {            
            return View();
        }

        public ActionResult Create()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Delete()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Edit()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Show()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}