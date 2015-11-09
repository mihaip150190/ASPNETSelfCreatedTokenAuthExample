using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TokenAuthExampleWebApplication.Authentication
{
    public class BasicUser
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleID { get; set; }
    }
}
