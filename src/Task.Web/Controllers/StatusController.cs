using Microsoft.AspNetCore.Mvc;
using Shared.Web.Extensions;

namespace TaskManager.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        private static readonly string _version = typeof(StatusController).Assembly.GetName().Version?.ToString();

        [HttpGet]
        public ActionResult Index()
        {
            var ip = Request.ToClientIp();
            var version = _version;
            return Ok(new { version, ip });
        }
    }
}
