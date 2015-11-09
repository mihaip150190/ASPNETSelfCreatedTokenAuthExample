using System;
using System.Collections.Generic;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;

namespace TokenAuthExampleWebApplication.AuthModels
{
    public class Role
    {
        public Role()
        {
            User = new HashSet<User>();
        }

        public int RoleID { get; set; }
        public string Description { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
}
