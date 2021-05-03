using Prometheus;

namespace PaymentGateway
{
    interface IMetricsService
    {
        Histogram RequestDurationMiliseconds { get; }

        Counter UnhandledExceptionsTotal { get; }

        Counter RequestsCount { get; }
    }
}
