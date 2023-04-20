namespace Meteor.Controller.Infrastructure.Services.Contracts;

public interface IMigrationRunner
{
    Task RunMigrationsAsync(CancellationToken cancellationToken = new());
}