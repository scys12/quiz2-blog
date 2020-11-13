using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Text;
using System.Web.Security;
using blog.Models.POCO;
using blog.Models.Repository;

namespace blog.Controllers
{
    public class PostController : Controller
    {
        private PostRepository postRepository = new PostRepository();
        private CommentRepository commentRepository = new CommentRepository();

        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            if ((string)TempData["message"] != "")
            {
                ViewBag.Message = TempData["message"];
                ViewBag.Status = TempData["status"];
            }
            List<Post> posts = postRepository.GetUserPosts((int)Session["UserID"]);
            return View(posts);
        }

        [HttpGet]
        [Authorize]
        public ActionResult UserPost(int? id)
        {
            if (id != null)
            {
                List<Post> posts = postRepository.GetUserPosts(id.Value);
                return View(posts);
            }
            return HttpNotFound();
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateComment(int? id, string description)
        {
            string message = "";
            string status = "";
            if (ModelState.IsValid)
            {
                var comment = new Comment();
                comment.UserID = (int)Session["UserID"];
                comment.PostID = id.Value;
                comment.Description = description;
                commentRepository.CreateComment(comment);
                ModelState.Clear();
                message = "comment successfully inserted";
                status = "success";
            }
            else
            {
                message = "Error creating comment";
                status = "danger";
            }
            TempData["message"]= message;
            TempData["status"] = status;
            return RedirectToAction("Show", "Post", new { id=id });
        }

        [HttpGet]
        [Authorize]
        public ActionResult UpdateComment(int? id)
        {
            if (id != null)
            {
                var comment = commentRepository.GetComment(id.Value);
                if (comment != null && comment.User.Email == HttpContext.User.Identity.Name)
                {
                    return View(comment);
                }
                else
                {
                    return HttpNotFound();
                }                
            }
            return HttpNotFound();

        }

        [HttpPost]
        [Authorize]
        public ActionResult UpdateComment(int? id, Comment comment)
        {
            string message = "";
            string status = "";
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    comment.UserID = (int)Session["UserID"];
                    comment.CommentID = id.Value;
                    commentRepository.UpdateComment(comment);
                    ModelState.Clear();
                    message = "comment successfully updated";
                    status = "success";

                    TempData["message"] = message;
                    TempData["status"] = status;
                    return RedirectToAction("Show", "Post", new { id = comment.PostID });
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                message = "Error updating comment";
                status = "danger";
            }
            TempData["message"] = message;
            TempData["status"] = status;
            return RedirectToAction("Show", "Post", new { id = comment.PostID });
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteComment(int? id, int postID)
        {
            string message = "";
            string status = "";
            if (id != null)
            {
                var isDelete = commentRepository.DeleteComment(id.Value);
                if (isDelete)
                {
                    message = "comment successfully deleted";
                    status = "success";
                    ModelState.Clear();
                }
                else
                {
                    message = "Error deleting comment";
                    status = "danger";
                }
                TempData["message"] = message;
                TempData["status"] = status;
                return RedirectToAction("Show", "Post", new { id = postID});
            }
            return HttpNotFound();

        }

        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(Post post)
        {
            string message = "";
            string status = "";
            if (ModelState.IsValid)
            {
                post.UserID = (int) Session["UserID"];
                postRepository.CreatePost(post);
                message = "Post successfully inserted";
                status = "success";
                ModelState.Clear();
            }
            else
            {
                message = "Error creating post";
                status = "danger";
                ViewBag.Message = message;
                ViewBag.Status = status;
                return View();
            }
            TempData["message"] = message;
            TempData["status"] = status;
            return RedirectToAction("Index", "Post");
        }

        [HttpPost]
        [Authorize]
        public ActionResult Delete(int? id)
        {
            string message = "";
            string status= "";
            if (id != null)
            {
                var isDelete = postRepository.DeletePost(id.Value);
                if (isDelete)
                {
                    message = "post successfully deleted";
                    status = "success";
                }
                else
                {
                    message = "error deleting post";
                    status = "danger";
                }
            }
            TempData["message"]= message;
            TempData["status"] = status;
            return RedirectToAction("Index", "Post");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Edit(int? id)
        {            
            if (id != null)
            {

                Post post = postRepository.GetPost(id.Value);
                var userId = (int) Session["UserID"];
                if (post != null && post.UserID == userId)
                {
                    return View(post);
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(int? id, Post post)
        {            
            string message = "";
            string status = "";                
            if (id != null)
            {
                post.PostID = id.Value;
                var userId = (int)Session["UserID"];
                if (post.UserID == userId)
                {
                    var isUpdate = postRepository.UpdatePost(post);
                    if (isUpdate)
                    {
                        message = "Post successfully updated";
                        status = "success";
                        TempData["message"] = message;
                        TempData["status"] = status;
                        return RedirectToAction("Index", "Post");
                    }
                }
            }
            return View(post);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Show(int? id)
        {
            if ((string)TempData["message"] != "")
            {
                ViewBag.Message = TempData["message"];
                ViewBag.Status = TempData["status"];
            }
            if (id != null)
            {
                Post post = postRepository.GetPost(id.Value);
                if (post != null)
                {                    
                    List<Comment> comments = commentRepository.commentsPost(post.PostID);
                    return View(Tuple.Create(post, comments));
                }
            }
            return HttpNotFound();
        }
    }
}