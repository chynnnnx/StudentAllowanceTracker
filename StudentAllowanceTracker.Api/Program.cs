using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using StudentAllowanceTracker.Api.Extensions;
using StudentAllowanceTracker.Application;
using StudentAllowanceTracker.Infrastructure;


var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog((context, cfg) => cfg.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDatabaseAndIdentity(builder.Configuration)
                 .AddApplication()
                .AddInfrastructure(builder.Configuration)
                .AddJwtAuthentication(builder.Configuration)
                .AddSwaggerDocumentation()
                .AddCustomCors(builder.Configuration); ;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

var app = builder.Build();
app.UseApplicationPipeline();
app.Run();