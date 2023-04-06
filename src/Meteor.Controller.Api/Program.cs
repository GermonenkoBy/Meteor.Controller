using Meteor.Controller.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

var builder = WebApplication.CreateBuilder(args);

var azureAppConfigurationConnectionString = builder.Configuration.GetValue<string>(
    "ConnectionStrings:AzureAppConfiguration"
);
if (!string.IsNullOrEmpty(azureAppConfigurationConnectionString))
{
    builder.Configuration.AddAzureAppConfiguration(options =>
        options
            .Connect(azureAppConfigurationConnectionString)
            .Select(KeyFilter.Any, KeyFilter.Any)
            .Select(KeyFilter.Any, builder.Environment.EnvironmentName)
    );
}

builder.Services.AddGrpc();

var connectionString = builder.Configuration.GetConnectionString("Controller");
builder.Services.AddDbContext<ControllerContext>(
    options => options
        .UseNpgsql(connectionString, opt => opt.MigrationsAssembly("Meteor.Controller.Migrations"))
        .UseSnakeCaseNamingConvention()
);

var app = builder.Build();

app.Run();