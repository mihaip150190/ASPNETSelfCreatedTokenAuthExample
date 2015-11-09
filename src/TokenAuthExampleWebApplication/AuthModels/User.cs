using System;
using System.Collections.Generic;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;

namespace TokenAuthExampleWebApplication.AuthModels
{
    public class User
    {
        public int UserID { get; set; }
        public string CustomProperty { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleID { get; set; }
        public string Username { get; set; }

        public virtual Role Role { get; set; }
    }
}
