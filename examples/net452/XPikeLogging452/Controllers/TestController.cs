using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Example.Library;
using XPike.Logging;

namespace XPikeLogging452.Controllers
{
    [Route("test")]
    public class TestController : ApiController
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

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> Test()
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