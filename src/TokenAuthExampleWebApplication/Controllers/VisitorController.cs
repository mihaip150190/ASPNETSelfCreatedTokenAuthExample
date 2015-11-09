using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TokenAuthExampleWebApplication.Controllers
{
    [Authorize("Bearer", Roles = "Visitor")]
    [Route("api/[controller]")]
    public class VisitorController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "This is only accessible to visitor" };
        }
    }
}
