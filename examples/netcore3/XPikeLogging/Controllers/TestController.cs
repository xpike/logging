using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Example.Library;
using Microsoft.AspNetCore.Mvc;
using XPike.Logging;

namespace XPikeLogging.Controllers
{
    [Route("test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILog<TestController> _logger;
        private readonly ITraceContext _traceContext;
        private readonly ITraceContextAccessor _accessor;
        private readonly ITestModule _testModule;

        public TestController(ILog<TestController> logger, ITraceContext traceContext, ITraceContextAccessor accessor, ITestModule testModule)
        {
            _logger = logger;
            _traceContext = traceContext;
            _accessor = accessor;
            _testModule = testModule;
        }

        [HttpGet("")]
        public async Task<IActionResult> Test()
        {
            _traceContext.Set("controller", GetType().ToString());
            await _testModule.DoThings();
            _traceContext.Set("completed", "things");

            var ctx = _accessor.TraceContext;
            ctx.Set("double", "checked");

            _logger.Log(string.Join(";", Request.Headers.Select(x => $"{x.Key}={string.Join(",", x.Value.ToList())}")),
                new Dictionary<string, string>
                {
                    {"endpoint", nameof(Test)}
                });

            return Ok("Log generated.");
        }
    }
}