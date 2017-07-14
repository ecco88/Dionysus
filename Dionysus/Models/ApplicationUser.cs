using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Dionysus.Models
{
    public class ApplicationUser : ClaimsPrincipal
    {
        
    }

    public class Base
    {
        public int ID { get; set; }
    }
    public class User : Base
    {
        public string Email { get; set; }
        public string Password { set; get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public User()
        {
            ID = -2;        // Test ID
            Password = "test";
            FirstName = "Some"
                ;
            LastName = "User";
            Email = "test@test.com";
            }
        public User(string email, string password)
        {
            Email = email;
            Password = password;
            ID = -2;        // Test ID
            FirstName = "Some"                ;
            LastName = "User";
        }
    }
}
