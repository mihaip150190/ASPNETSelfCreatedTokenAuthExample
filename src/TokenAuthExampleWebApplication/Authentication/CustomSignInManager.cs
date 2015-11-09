using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Http;
using Microsoft.Framework.OptionsModel;
using Microsoft.AspNet.Http.Authentication;
using System.Security.Claims;
using Microsoft.AspNet.Authentication.JwtBearer;

namespace TokenAuthExampleWebApplication.Authentication
{
    public class CustomSignInManager<TUser> : ISignInManager<TUser> where TUser : BasicUser
    {
        private HttpContext _context;
        private IdentityOptions _options;
        private IUserManager<TUser> _userManager;

        public CustomSignInManager(IHttpContextAccessor contextAccessor, IOptions<IdentityOptions> optionsAccessor, IUserManager<TUser> userManager)
        {
            if (contextAccessor == null || contextAccessor.HttpContext == null)
            {
                throw new ArgumentNullException(nameof(contextAccessor));
            }

            _context = contextAccessor.HttpContext;
            _options = optionsAccessor?.Value ?? new IdentityOptions();
            _userManager = userManager;
        }

        public async Task<AuthenticationResult> PasswordSignInAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new AuthenticationResult { Result = SignInResult.Failed, Identity = null };
            }

            return await PasswordSignInAsync(user, password);
        }

       
        public async Task<ClaimsIdentity> SignInAsync(TUser user)
        {
            return await CreateUserPrincipalAsync(user);
        }

        public async Task SignOutAsync()
        {
            await _context.Authentication.SignOutAsync(_options.Cookies.ApplicationCookieAuthenticationScheme);
        }

        private async Task<ClaimsIdentity> CreateUserPrincipalAsync(BasicUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var id = new ClaimsIdentity(_options.Cookies.ApplicationCookieAuthenticationScheme,
                                        _options.ClaimsIdentity.UserNameClaimType,
                                        _options.ClaimsIdentity.RoleClaimType);
            id.AddClaim(new Claim(_options.ClaimsIdentity.UserIdClaimType, user.UserId.ToString()));
            id.AddClaim(new Claim(_options.ClaimsIdentity.UserNameClaimType, user.Username));
            id.AddClaim(new Claim(_options.ClaimsIdentity.RoleClaimType, await _userManager.GetRoleByIdAsync(user.RoleID)));
            return id;
        }

        private async Task<AuthenticationResult> PasswordSignInAsync(TUser user, string password)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (_userManager.CheckPassword(user, password))
            {
                var identity = await SignInAsync(user);
                return new AuthenticationResult { Result = SignInResult.Success, Identity = identity};
            }

            return new AuthenticationResult { Result = SignInResult.Failed, Identity = null };
        }
    }
}
