using Meteor.Controller.Core;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Meteor.Controller.Api.HealthChecks;

public class DatabaseConnectionHealthcheck : IHealthCheck
{
    private readonly ControllerContext _context;

    public DatabaseConnectionHealthcheck(ControllerContext context)
    {
        _context = context;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = new ()
    )
    {
        return await _context.Database.CanConnectAsync(cancellationToken)
            ? HealthCheckResult.Healthy("Database is online.")
            : HealthCheckResult.Unhealthy("Failed to establish database connection.");
    }
}