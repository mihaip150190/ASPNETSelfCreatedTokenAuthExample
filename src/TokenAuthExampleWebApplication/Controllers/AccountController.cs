using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ModelBinding;
using System.Threading.Tasks;
using TokenAuthExampleWebApplication.Authentication;
using TokenAuthExampleWebApplication.ViewModels;

namespace TokenAuthExampleWebApplication.Controllers
{
    public class AccountController : Controller
    {
        private IUserManager<CustomUser> _userManager;

        public AccountController(IUserManager<CustomUser> userManager)
        {
            _userManager = userManager;
            //this.bearerOptions = options.Value;
            //this.signingCredentials = signingCredentials;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new CustomUser { Username = model.Email, RoleID = 1, Email = model.Email, CustomProperty = "test" };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return new HttpOkResult();
                }
            }

            // If we got this far, something failed, redisplay form
            return HttpUnauthorized();
        }
    }
}
