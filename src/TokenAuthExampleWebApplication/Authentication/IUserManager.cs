using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TokenAuthExampleWebApplication.Authentication
{
    public interface IUserManager<TUser> where TUser : BasicUser
    {
         Task<IdentityResult> CreateAsync(TUser user, string password);
         Task<string> GetRoleByIdAsync(int roleId);
         Task<TUser> FindByNameAsync(string userName);
         bool CheckPassword(TUser user, string password);
    }
}
