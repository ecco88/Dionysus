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
                result.ID = Convert.ToInt32(dr["UserID"]);
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

        public async Task<int> Register(string FirstName, string LastName, string Email)
        {
            int result = -1;
            using (DAL dal = new DAL())
            {
                command = new SqlCommand("Register", conn);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FirstName", FirstName);
                command.Parameters.AddWithValue("@LastName", LastName);
                command.Parameters.AddWithValue("@Email", Email);
                command.Parameters.AddWithValue("@Password", "password");
                command.Connection.Open();
                result = await command.ExecuteNonQueryAsync();
                command.Connection.Close();
                return result;
            }
        }

        public async Task<Models.User> LogIn(string Email, string password)
        {
            command = new SqlCommand("LogIn", conn) { CommandType = System.Data.CommandType.StoredProcedure};
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Password", password);
            command.Connection.Open();
            var dr = await command.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
            return await dr.GetMember();
        }


    }
}
