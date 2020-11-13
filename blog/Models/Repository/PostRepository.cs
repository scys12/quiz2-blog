using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using blog.Models.POCO;

namespace blog.Models.Repository
{
    public class PostRepository
    {
        private SqlConnection con;
        private UserRepository userRepository = new UserRepository();
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["blog"].ToString();
            con = new SqlConnection(constring);
        }

        public List<Post> GetAllPosts()
        {
            connection();
            List<Post> postList = new List<Post>();
            string query = "SELECT * FROM Posts";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                postList.Add(
                    new Post
                    {
                        PostID = Convert.ToInt32(dr["PostID"]),
                        Title = Convert.ToString(dr["Title"]),
                        Description = Convert.ToString(dr["Description"]),
                        UserID = Convert.ToInt32(dr["UserID"]),
                        User = userRepository.GetUserDetail(Convert.ToInt32(dr["UserID"]))
                    }
                );
            }
            return postList;
        }

        public List<Post> GetUserPosts(int userID)
        {
            connection();
            List<Post> postList = new List<Post>();
            string query = "SELECT * FROM Posts WHERE UserID=@user_id";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@user_id", userID);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                postList.Add(
                    new Post
                    {
                        PostID = Convert.ToInt32(dr["PostID"]),
                        Title = Convert.ToString(dr["Title"]),
                        Description = Convert.ToString(dr["Description"]),
                        UserID = Convert.ToInt32(dr["UserID"]),
                        User = userRepository.GetUserDetail(userID)
                    }
                );
            }
            return postList;
        }

        public bool CreatePost(Post post)
        {
            connection();
            string query = "INSERT INTO Posts VALUES(@user_id, @title, @description)";
            SqlCommand sqlCommand = new SqlCommand(query, con);
            sqlCommand.Parameters.AddWithValue("@user_id", post.UserID);
            sqlCommand.Parameters.AddWithValue("@title", post.Title);
            sqlCommand.Parameters.AddWithValue("@description", post.Description);

            con.Open();
            int i = sqlCommand.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        public bool UpdatePost(Post post)
        {
            connection();
            string query = "UPDATE Posts SET UserID=@user_id, Description=@description, Title=@title WHERE PostID=@post_id";
            SqlCommand sqlCommand = new SqlCommand(query, con);
            sqlCommand.Parameters.AddWithValue("@post_id", post.PostID);
            sqlCommand.Parameters.AddWithValue("@user_id", post.UserID);
            sqlCommand.Parameters.AddWithValue("@title", post.Title);
            sqlCommand.Parameters.AddWithValue("@description", post.Description);

            con.Open();
            int i = sqlCommand.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        public bool DeletePost(int postID)
        {
            connection();
            string query = "DELETE Posts WHERE PostID=@post_id";
            SqlCommand sqlCommand = new SqlCommand(query, con);
            sqlCommand.Parameters.AddWithValue("@post_id", postID);

            con.Open();
            int i = sqlCommand.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        public Post GetPost(int postID)
        {
            connection();
            Post post = new Post();
            bool status = false;
            string query = "SELECT * FROM Posts WHERE PostID=@post_id";
            SqlCommand sqlCommand = new SqlCommand(query, con);
            sqlCommand.Parameters.AddWithValue("@post_id", postID);
            SqlDataAdapter sd = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            if (dt.Rows.Count == 1)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    post.PostID = Convert.ToInt32(dr["PostID"]);
                    post.Title = Convert.ToString(dr["Title"]);
                    post.Description = Convert.ToString(dr["Description"]);
                    post.UserID = Convert.ToInt32(dr["UserID"]);
                    post.User = userRepository.GetUserDetail(post.UserID);
                }
                status = true;
            }
            if (status)
            {
                return post;
            }
            else
            {
                return null;
            }
        }
    }    
}