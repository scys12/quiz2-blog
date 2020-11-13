using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using blog.Models.Repository;
using blog.Models.POCO;
using System.Web.Mvc;

namespace blog.Controllers
{
    public class HomeController : Controller
    {
        private PostRepository postRepository = new PostRepository();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult AllPost()
        {
            List<Post> posts = postRepository.GetAllPosts();
            return View(posts);
        }
    }
}