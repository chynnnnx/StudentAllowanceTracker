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
                .AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomCors(builder.Configuration);

var app = builder.Build();
app.UseCustomCors();
app.UseApplicationPipeline();
app.MapControllers();
app.Run();