using Meteor.Controller.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;

namespace Meteor.Controller.Api.Extensions;

public static class WebApplicationExtensions
{
    public static async Task RunMigrations(this WebApplication app)
    {
        var featureManager = app.Services.GetRequiredService<IFeatureManager>();
        var runMigrations = await featureManager.IsEnabledAsync("RunMigrationsOnStartup");
        if (!runMigrations)
        {
            return;
        }

        using var scope = app.Services.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ControllerContext>();
        await context.Database.MigrateAsync();
    }
}