using Meteor.Controller.Api.Enums;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Meteor.Controller.Api.HealthChecks;

public class MigrationsHealthcheck : IHealthCheck
{
    public MigrationsStatuses MigrationsStatus { get; set; } = MigrationsStatuses.NotStarted;
    
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = new ()
    )
    {
        if (MigrationsStatus is MigrationsStatuses.Completed or MigrationsStatuses.Disabled)
        {
            return Task.FromResult(HealthCheckResult.Healthy("Migrations are completed."));
        }
        
        if (MigrationsStatus is MigrationsStatuses.NotStarted or MigrationsStatuses.InProgress)
        {
            return Task.FromResult(HealthCheckResult.Degraded("Migrations are in progress"));
        }

        return Task.FromResult(HealthCheckResult.Unhealthy("Migrations were failed to run"));
    }
}