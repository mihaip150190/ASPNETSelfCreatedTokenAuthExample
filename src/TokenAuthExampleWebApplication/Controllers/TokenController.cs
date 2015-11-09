using System;
using Microsoft.AspNet.Mvc;
using System.Security.Claims;
using Microsoft.AspNet.Authorization;
using System.Security.Principal;
using System.IdentityModel.Tokens.Jwt;
using TokenAuthExampleWebApplication.Authentication;
using TokenAuthExampleWebApplication.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace TokenAuthExampleWebApplication.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly TokenAuthOptions tokenOptions;
        private ISignInManager<CustomUser> _signInManager;

        public TokenController(TokenAuthOptions tokenOptions, ISignInManager<CustomUser> signInManager)
        {
            this.tokenOptions = tokenOptions;
            _signInManager = signInManager;
            //this.bearerOptions = options.Value;
            //this.signingCredentials = signingCredentials;
        }

        /// <summary>
        /// Check if currently authenticated. Will throw an exception of some sort which shoudl be caught by a general
        /// exception handler and returned to the user as a 401, if not authenticated. Will return a fresh token if
        /// the user is authenticated, which will reset the expiry.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Bearer")]
        public dynamic Get()
        {
            bool authenticated = false;
            string user = null;
            string token = null;
            DateTime? tokenExpires = default(DateTime?);

            var currentUser = HttpContext.User;
            if (currentUser != null)
            {
                authenticated = currentUser.Identity.IsAuthenticated;
                if (authenticated)
                {
                    user = currentUser.Identity.Name;
                    tokenExpires = DateTime.UtcNow.AddMinutes(2);
                    token = GetToken(currentUser.Identity as ClaimsIdentity, tokenExpires);
                }
            }
            return new { authenticated = authenticated, user = user, token = token, tokenExpires = tokenExpires };
        }

        public class AuthRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        /// <summary>
        /// Request a new token for a given username/password pair.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<dynamic> Post([FromBody] AuthRequest req)
        {
            var result = await _signInManager.PasswordSignInAsync(req.Email, req.Password);
            // Obviously, at this point you need to validate the username and password against whatever system you wish.
            if (result.Result == SignInResult.Success)
            {
                DateTime? expires = DateTime.UtcNow.AddMinutes(2);
                var token = GetToken(result.Identity, expires);
                return new { authenticated = true, entityId = 1, token = token, tokenExpires = expires };
            }
            return new { authenticated = false };
        }

        private string GetToken(ClaimsIdentity identity, DateTime? expires)
        {
            var handler = new JwtSecurityTokenHandler();

            var securityToken = handler.CreateToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                signingCredentials: tokenOptions.SigningCredentials,
                subject: identity,
                expires: expires
                );
            return handler.WriteToken(securityToken);
        }
    }
}
