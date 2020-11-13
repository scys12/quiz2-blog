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
    public class HomeController : Controller
    {
        private string CS = ConfigurationManager.ConnectionStrings["blog"].ConnectionString;
        public ActionResult Index()
        {
            /*List<Post> postList = new List<Post>();            
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Post", con);
                cmd.CommandType = CommandType.Text;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var post = new Post();
                    post.PostID= Convert.ToInt32(rdr["PostID"]);
                    post.Title= rdr["Title"].ToString();
                    post.Description = rdr["Description"].ToString();
                    post.UserID = Convert.ToInt32(rdr["UserID"]);
                    postList.Add(post);
                }
            }*/
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
    }
}