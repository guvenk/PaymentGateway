using Prometheus;

namespace PaymentGateway
{
    public class MetricsService : IMetricsService
    {
        private const string ApplicationPrefix = "payment_gateway";

        public MetricsService()
        {
            RequestDurationMiliseconds = Metrics.CreateHistogram(
                name: $"{ApplicationPrefix}_http_request_duration_seconds",
                help: "Total duration of http request",
                labelNames: new string[] { "method", "handler", "code" });

            UnhandledExceptionsTotal = Metrics.CreateCounter(
                name: $"{ApplicationPrefix}_unhandled_exceptions_total",
                help: "Total count of unhandled exceptions");

            RequestsCount = Metrics.CreateCounter(
                name: $"{ApplicationPrefix}_requests_count",
                help: "Total requests number",
                labelNames: new string[] { "endpoint" });
        }

        public Histogram RequestDurationMiliseconds { get; private set; }

        public Counter UnhandledExceptionsTotal { get; private set; }

        public Counter RequestsCount { get; private set; }
    }
}
