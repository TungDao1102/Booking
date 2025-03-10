using Serilog.Context;

namespace Booking.API.Middleware
{
    /// <summary>
    /// trace log request 
    /// </summary>
    public class RequestContextLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private const string CorrelationIdHeaderName = "X-Correlation-Id";
        public RequestContextLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            using (LogContext.PushProperty("CorrelationId", GetCorrelationId(context)))
            {
                return _next(context);
            }
        }

        private static string GetCorrelationId(HttpContext context)
        {
            context.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var correlationId);
            return correlationId.FirstOrDefault() ?? context.TraceIdentifier;
        }
    }
}
