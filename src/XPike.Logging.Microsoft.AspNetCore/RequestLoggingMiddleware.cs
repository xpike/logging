using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using XPike.Configuration;

namespace XPike.Logging.Microsoft.AspNetCore
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogService _logService;
        private readonly IConfig<LogServiceConfig> _config;
        private readonly ITraceContextAccessor _contextAccessor;

        public RequestLoggingMiddleware(RequestDelegate next, ILogService logService, IConfig<LogServiceConfig> config, ITraceContextAccessor contextAccessor)
        {
            _next = next;
            _logService = logService;
            _config = config;
            _contextAccessor = contextAccessor;
        }

        private Dictionary<string, string> GetTags(HttpContext context)
        {
            var tags = new Dictionary<string, string>();

            try
            {
                tags["traceId"] = context.TraceIdentifier;
                tags["connectionId"] = context.Connection.Id;

                if (context.Connection.ClientCertificate != null)
                    tags["clientCertificate"] = context.Connection.ClientCertificate.Thumbprint;

                if (context.Features?.Any() ?? false)
                    tags["contextFeatures"] = string.Join(";", context.Features.Select(x => x.Key.Name));

                tags["contentType"] = context.Request.ContentType;
                tags["contentLength"] = context.Request.ContentLength.GetValueOrDefault(0).ToString();
                tags["method"] = context.Request.Method;
                tags["path"] = context.Request.Path;
                tags["protocol"] = context.Request.Protocol;
                tags["scheme"] = context.Request.Scheme;
                tags["host"] = context.Request.Host.ToUriComponent();

                if (context.Response?.HasStarted ?? false)
                {
                    tags["responseType"] = context.Response.ContentType;
                    tags["responseLength"] = context.Response.ContentLength.GetValueOrDefault(0).ToString();
                    tags["responseCode"] = context.Response.StatusCode.ToString();
                }

                //if (context.Session?.IsAvailable ?? false)
                //    tags["sessionId"] = context.Session.Id;

                if (context.User?.Identity != null)
                {
                    tags["identity"] = context.User.Identity.IsAuthenticated.ToString();
                    tags["authType"] = context.User.Identity.AuthenticationType;
                    tags["identity"] = context.User.Identity.Name;
                }

                if (context.User?.Claims?.Any() ?? false)
                    tags["claims"] = string.Join(";", context.User.Claims.Select(x => $"{x.Type}={x.Value}"));
            }
            catch (Exception ex)
            {
                _logService.Warn($"Failed to prepare diagnostic tags for request logging: {ex.Message} ({ex.GetType()})",
                    ex,
                    tags,
                    nameof(RequestLoggingMiddleware));
            }

            return tags;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!_config.CurrentValue.EnableRequestLogging)
            {
                await _next(context);
                return;
            }

            var tags = GetTags(context);
            if (!tags.TryGetValue("path", out var path))
                path = "(unknown)";

            try
            {
                // NOTE: It's important to attempt to acquire the trace context here, otherwise
                // trace items added by controller logic won't be recorded in these log entries.
                var traceContext = _contextAccessor.TraceContext;
            }
            catch (Exception)
            {
                // Intentional no-op.
            }

            _logService.Trace($"Begin request processing: {path}", tags, nameof(RequestLoggingMiddleware));

            try
            {
                await _next(context);
                _logService.Trace($"Request processing completed: {path}", tags, nameof(RequestLoggingMiddleware));
            }
            catch (Exception ex)
            {
                _logService.Error($"Request processing failed ({path}): {ex.Message} ({ex.GetType()})", ex, tags, nameof(RequestLoggingMiddleware));
                throw;
            }
        }
    }
}