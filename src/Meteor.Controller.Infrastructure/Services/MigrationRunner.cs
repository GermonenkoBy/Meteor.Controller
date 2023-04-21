using Meteor.Controller.Core;
using Meteor.Controller.Infrastructure.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Meteor.Controller.Infrastructure.Services;

public class MigrationRunner : IMigrationRunner
{
    private readonly ControllerContext _context;

    private readonly ILogger<MigrationRunner> _logger;

    public MigrationRunner(ControllerContext context, ILogger<MigrationRunner> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task RunMigrationsAsync(CancellationToken cancellationToken = new())
    {
        _logger.LogInformation("Applying migrations");
        try
        {
            await _context.Database.MigrateAsync(cancellationToken);
            _logger.LogInformation("Migrations were applied successfully");
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "Failed to apply migrations");
            throw;
        }
    }
}