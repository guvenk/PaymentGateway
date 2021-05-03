using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;

namespace PaymentGateway
{
    internal class MetricsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMetricsService _metrics;

        public MetricsMiddleware(RequestDelegate next, IMetricsService metrics)
        {
            _next = next;
            _metrics = metrics;
        }

        public async Task Invoke(HttpContext context)
        {
            string requestMethod = context.Request.Method;
            string requestPath = context.Request.Path;

            _metrics.RequestsCount.Labels(requestPath).Inc();

            Stopwatch timer = Stopwatch.StartNew();

            await _next(context);

            timer.Stop();

            string statusCode = context.Response.StatusCode.ToString(CultureInfo.InvariantCulture);

            _metrics.RequestDurationMiliseconds
                .Labels(requestMethod, requestPath, statusCode)
                .Observe(timer.ElapsedMilliseconds);
        }
    }
}
