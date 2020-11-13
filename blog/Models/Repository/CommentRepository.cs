using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using blog.Models.POCO;

namespace blog.Models.Repository
{
    public class CommentRepository
    {
        private SqlConnection con;
        private UserRepository userRepository = new UserRepository();
        private PostRepository postRepository = new PostRepository();
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["blog"].ToString();
            con = new SqlConnection(constring);
        }
        
        public List<Comment> GetUserComments(int userID)
        {
            connection();
            List<Comment> commentList = new List<Comment>();
            string query = "SELECT * FROM Comments WHERE UserID=@user_id";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@user_id", userID);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                commentList.Add(
                    new Comment
                    {
                        CommentID = Convert.ToInt32(dr["CommentID"]),
                        PostID = Convert.ToInt32(dr["PostID"]),
                        Post = postRepository.GetPost(Convert.ToInt32(dr["PostID"])),
                        Description = Convert.ToString(dr["Description"]),
                        UserID = Convert.ToInt32(dr["UserID"]),
                        User = userRepository.GetUserDetail(userID)
                    }
                );
            }
            return commentList;
        }

        public Comment GetComment(int commentID)
        {
            connection();
            Comment comment = new Comment();
            string query = "SELECT * FROM Comments WHERE CommentID=@comment_id";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@comment_id", commentID);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            if (dt.Rows.Count == 1)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    comment.CommentID = Convert.ToInt32(dr["CommentID"]);
                    comment.PostID = Convert.ToInt32(dr["PostID"]);
                    comment.Post = postRepository.GetPost(Convert.ToInt32(dr["PostID"]));
                    comment.Description = Convert.ToString(dr["Description"]);
                    comment.UserID = Convert.ToInt32(dr["UserID"]);
                    comment.User = userRepository.GetUserDetail(comment.UserID);
                }
                return comment;
            }
            return null;
        }

        public List<Comment> commentsPost(int postID)
        {
            connection();
            List<Comment> commentList = new List<Comment>();
            string query = "SELECT * FROM Comments WHERE PostID=@post_id";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@post_id", postID);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                commentList.Add(
                    new Comment
                    {
                        CommentID = Convert.ToInt32(dr["CommentID"]),
                        PostID = Convert.ToInt32(dr["PostID"]),
                        Post = postRepository.GetPost(Convert.ToInt32(dr["PostID"])),
                        Description = Convert.ToString(dr["Description"]),
                        UserID = Convert.ToInt32(dr["UserID"]),
                        User = userRepository.GetUserDetail(Convert.ToInt32(dr["UserID"]))
                    }
                );
            }
            return commentList;
        }

        public bool CreateComment(Comment comment)
        {
            connection();
            string query = "INSERT INTO Comments VALUES(@post_id, @user_id, @description)";
            SqlCommand sqlCommand = new SqlCommand(query, con);
            sqlCommand.Parameters.AddWithValue("@post_id", comment.PostID);
            sqlCommand.Parameters.AddWithValue("@user_id", comment.UserID);
            sqlCommand.Parameters.AddWithValue("@description", comment.Description);

            con.Open();
            int i = sqlCommand.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        public bool UpdateComment(Comment comment)
        {
            connection();
            string query = "UPDATE Comments SET PostID=@post_id, UserID=@user_id, Description=@description WHERE CommentID=@comment_id";
            SqlCommand sqlCommand = new SqlCommand(query, con);
            sqlCommand.Parameters.AddWithValue("@post_id", comment.PostID);
            sqlCommand.Parameters.AddWithValue("@user_id", comment.UserID);
            sqlCommand.Parameters.AddWithValue("@description", comment.Description);
            sqlCommand.Parameters.AddWithValue("@comment_id", comment.CommentID);

            con.Open();
            int i = sqlCommand.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        public bool DeleteComment(int postID)
        {
            connection();
            string query = "DELETE Comments WHERE CommentID=@comment_id";
            SqlCommand sqlCommand = new SqlCommand(query, con);
            sqlCommand.Parameters.AddWithValue("@comment_id", postID);

            con.Open();
            int i = sqlCommand.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }        
    }
}