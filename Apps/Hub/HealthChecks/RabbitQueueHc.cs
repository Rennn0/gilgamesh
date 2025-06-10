using Hub.Backgrounds;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Hub.HealthChecks
{
    public class RabbitQueueHc : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                if (RabbitQueueWorker.IsConnected)
                {
                    return Task.FromResult(HealthCheckResult.Healthy("RabbitMQ consumer is connected and ready."));
                }
                else
                {
                    return Task.FromResult(HealthCheckResult.Unhealthy("RabbitMQ consumer is not connected."));
                }
            }
            catch (Exception ex)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("An error occurred during RabbitMQ health check.", ex));
            }
        }
    }
}