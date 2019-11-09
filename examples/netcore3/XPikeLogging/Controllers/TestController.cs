using System.Linq;
using Microsoft.AspNetCore.Mvc;
using XPike.Logging;

namespace XPikeLogging.Controllers
{
    [Route("test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILog<TestController> _logger;

        public TestController(ILog<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult Test()
        {
            _logger.Log(string.Join(";", Request.Headers.Select(x => $"{x.Key}={string.Join(",", x.Value.ToList())}")));
            return Ok("Log generated.");
        }
    }
}