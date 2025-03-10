using Booking.Application.Abstractions.Data;
using Dapper;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Booking.Infrastructure.HealthChecks
{
    public class CustomSqlHealthCheck(ISqlConnectionFactory sqlConnection) : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using var connection = sqlConnection.CreateConnection();
                await connection.ExecuteScalarAsync("SELECT 1", cancellationToken);
                return HealthCheckResult.Healthy();
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy(exception: ex);
            }
        }
    }
}
