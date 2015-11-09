using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TokenAuthExampleWebApplication.Controllers
{
    [Authorize("Bearer", Roles = "Admin")]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "This is only accessible to admin" };
        }
    }
}
