using Mapster;
using MapsterMapper;
using Meteor.Common.Cryptography.DependencyInjection.Extensions;
using Meteor.Controller.Api.Extensions;
using Meteor.Controller.Api.Interceptors;
using Meteor.Controller.Api.Mapping;
using Meteor.Controller.Api.Services;
using Meteor.Controller.Core;
using Meteor.Controller.Core.Mapping;
using Meteor.Controller.Core.Services;
using Meteor.Controller.Core.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

var azureAppConfigurationConnectionString = builder.Configuration
    .GetValue<string>("ConnectionStrings:AzureAppConfiguration");

if (!string.IsNullOrEmpty(azureAppConfigurationConnectionString))
{
    builder.Configuration.AddAzureAppConfiguration(options =>
        options
            .Connect(azureAppConfigurationConnectionString)
            .UseFeatureFlags()
            .Select(KeyFilter.Any)
            .Select(KeyFilter.Any, builder.Environment.EnvironmentName)
            .Select(KeyFilter.Any, $"{builder.Environment.EnvironmentName}-Controller")
    );
}

builder.Services.AddFeatureManagement();

var connectionString = builder.Configuration.GetConnectionString("Controller");
builder.Services.AddDbContext<ControllerContext>(
    options => options
        .UseNpgsql(connectionString, opt => opt.MigrationsAssembly("Meteor.Controller.Migrations"))
        .UseSnakeCaseNamingConvention()
        .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
);

builder.Services.AddEncryptor(options =>
{
    var key = builder.Configuration["Security:Encryption:AesKey"] ?? "";
    var iv = builder.Configuration["Security:Encryption:Iv"] ?? "";
    options.AesKey = Convert.FromHexString(key);
    options.InitializationVector = Convert.FromHexString(iv);
});

var config = new TypeAdapterConfig();
config.Apply(new ApiMappingsRegister());
config.Apply(new CoreMappingsRegister());
builder.Services.AddSingleton<IMapper>(new Mapper(config));

builder.Services.AddGrpc(options => options.Interceptors.Add<ExceptionHandlingInterceptor>());
builder.Services.AddGrpcReflection();

builder.Services.AddScoped<ICustomersService, CustomersService>();

var app = builder.Build();
await app.RunMigrations();

app.MapGrpcReflectionService();
app.MapGrpcService<CustomersGrpcService>();
app.Run();