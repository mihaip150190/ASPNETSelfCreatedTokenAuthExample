using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TokenAuthExampleWebApplication.Authentication
{
    public interface ISignInManager<TUser> where TUser : BasicUser
    {
        Task<AuthenticationResult> PasswordSignInAsync(string email, string password);
        Task<ClaimsIdentity> SignInAsync(TUser user);
        Task SignOutAsync();
    }
}
