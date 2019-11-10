using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using XPike.Logging;

namespace Example.Library
{
    public class TestModule
        : ITestModule
    {
        private readonly ITraceContextAccessor _contextAccessor;

        public TestModule(ITraceContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public async Task DoThings()
        {
            var sw = Stopwatch.StartNew();

            var context = _contextAccessor.TraceContext;
            context.Set("timestamp", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));

            await Task.Delay(500);

            context = _contextAccessor.TraceContext;
            context.Set("elapsed", sw.Elapsed.ToString());
        }
    }
}