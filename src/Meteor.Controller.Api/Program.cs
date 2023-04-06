using Meteor.Controller.Core;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var connectionString = builder.Configuration.GetConnectionString("Controller");
builder.Services.AddDbContext<ControllerContext>(
    options => options
        .UseNpgsql(connectionString, opt => opt.MigrationsAssembly("Meteor.Controller.Migrations"))
        .UseSnakeCaseNamingConvention()
);

var app = builder.Build();

app.Run();