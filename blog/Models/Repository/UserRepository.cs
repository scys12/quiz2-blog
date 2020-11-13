using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using blog.Models.POCO;

namespace blog.Models.Repository
{
    public class UserRepository
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["blog"].ToString();
            con = new SqlConnection(constring);
        }

        public bool CheckEmailExist(string email)
        {
            connection();
            string query = "SELECT TOP 1 * FROM Users WHERE Email=@user_email";
            SqlCommand sqlCommand = new SqlCommand(query, con);
            sqlCommand.Parameters.AddWithValue("@user_email", email);
            SqlDataAdapter sd = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            if (dt.Rows.Count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RegisterUser(RegisterViewModel registerModel)
        {
            connection();
            string query = "INSERT INTO Users VALUES(@full_name, @email, @password)";
            SqlCommand sqlCommand = new SqlCommand(query, con);
            sqlCommand.Parameters.AddWithValue("@full_name", registerModel.FullName);
            sqlCommand.Parameters.AddWithValue("@email", registerModel.Email);
            sqlCommand.Parameters.AddWithValue("@password", registerModel.Password);            

            con.Open();
            int i = sqlCommand.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        public User Login(LoginViewModel loginModel)
        {
            connection();
            bool status = false;
            var user = new User();
            string query = "SELECT TOP 1 * FROM Users WHERE Email=@user_email ";
            string password ="";
            SqlCommand sqlCommand = new SqlCommand(query, con);
            sqlCommand.Parameters.AddWithValue("@user_email", loginModel.Email);
            SqlDataAdapter sd = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            if (dt.Rows.Count == 1)
            {                
                foreach (DataRow dataRow in dt.Rows)
                {
                    user.UserID = Convert.ToInt32(dataRow["UserID"]);
                    user.Email = Convert.ToString(dataRow["Email"]);
                    user.FullName = Convert.ToString(dataRow["FullName"]);
                    password = Convert.ToString(dataRow["Password"]);
                }
                if (string.Compare(loginModel.Password, password) == 0)
                {
                    status = true;
                }
            }
            if (status)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public User GetUserDetail(int userID)
        {
            connection();
            bool status = false;
            var user = new User();
            string query = "SELECT * FROM Users WHERE UserID=@user_id";
            SqlCommand sqlCommand = new SqlCommand(query, con);
            sqlCommand.Parameters.AddWithValue("@user_id", userID);
            SqlDataAdapter sd = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            if (dt.Rows.Count == 1)
            {
                foreach (DataRow dataRow in dt.Rows)
                {
                    user.UserID = Convert.ToInt32(dataRow["UserID"]);
                    user.Email = Convert.ToString(dataRow["Email"]);
                    user.FullName = Convert.ToString(dataRow["FullName"]);
                }
                status = true;
            }
            if (status)
            {
                return user;
            }
            else
            {
                return null;
            }
        }
    }
}