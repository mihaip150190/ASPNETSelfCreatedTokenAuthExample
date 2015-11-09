using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TokenAuthExampleWebApplication.Authentication
{
    public class AuthenticationResult
    {
        public SignInResult Result { get; set; }
        public ClaimsIdentity Identity { get; set; }
    }
}
