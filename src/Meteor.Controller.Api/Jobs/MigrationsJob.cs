using Meteor.Controller.Api.Enums;
using Meteor.Controller.Api.HealthChecks;
using Meteor.Controller.Infrastructure.Services.Contracts;
using Microsoft.FeatureManagement;

namespace Meteor.Controller.Api.Jobs;

public class MigrationsJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public MigrationsJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var scope = _serviceProvider.CreateScope();
        var featureManager = scope.ServiceProvider.GetRequiredService<IFeatureManager>();
        var migrationsHealthCheck = scope.ServiceProvider.GetRequiredService<MigrationsHealthcheck>();
        
        if (!await featureManager.IsEnabledAsync("RunMigrationsOnStartup"))
        {
            migrationsHealthCheck.MigrationsStatus = MigrationsStatuses.Disabled;
        }

        migrationsHealthCheck.MigrationsStatus = MigrationsStatuses.InProgress;
        
        var migrationsRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        
        try
        {
            await migrationsRunner.RunMigrationsAsync(stoppingToken);
            migrationsHealthCheck.MigrationsStatus = MigrationsStatuses.Completed;
        }
        catch(Exception)
        {
            migrationsHealthCheck.MigrationsStatus = MigrationsStatuses.Error;
        }
    }
}