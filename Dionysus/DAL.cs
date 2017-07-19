using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Dionysus.Models;

namespace Dionysus
{
    public static class DataReaderExtension
    {
        public static async Task<User> GetMember(this SqlDataReader dr)
        {
            User result = new User();
            bool  x = await dr.ReadAsync();
            if(x)
            {
                result.Email = dr["Email"].ToString();
                result.FirstName = dr["FirstName"].ToString();
                result.ID = Convert.ToInt32(dr["ID"]);
                result.LastName = dr["LastName"].ToString();
                
            }
            return result;
        }
    }
    public class DAL : IDisposable
    {
        public SqlConnection conn { get; set; }
        public SqlCommand command { get; set; }

        public void Dispose()
        {
            if (conn!=null && conn.State!=System.Data.ConnectionState.Closed)
            {
                conn.Close();
            }
        }

        public DAL() 
        {
            conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename='C:\\Users\\Brian Caputo\\Documents\\Visual Studio 2017\\Projects\\Dionysus\\Dionysus\\Data\\Dionysus.mdf';Integrated Security=True;Connect Timeout=30");
        }

        public async Task<User> Register(string FirstName, string LastName, string Email)
        {
            User result = new User();

            using (SqlCommand c = new SqlCommand("Register", conn) { CommandType = System.Data.CommandType.StoredProcedure })
            {                
                c.Parameters.AddWithValue("@FirstName", FirstName);
                c.Parameters.AddWithValue("@LastName", LastName);
                c.Parameters.AddWithValue("@Email", Email);
                c.Parameters.AddWithValue("@Password", "password");
                c.Connection.Open();
                SqlDataReader dr = await c.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                result = await dr.GetMember();
            }
            return result;
        }

        public async Task<Models.User> LogIn(string Email, string password)
        {
            User result = new User();
            using (SqlCommand c = new SqlCommand("LogIn", conn) { CommandType = System.Data.CommandType.StoredProcedure })
            {
                c.Parameters.AddWithValue("@Email", Email);
                c.Parameters.AddWithValue("@Password", password);
                c.Connection.Open();
                var dr = await c.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                result = await dr.GetMember();
            }
            return result;
        }


    }
}
