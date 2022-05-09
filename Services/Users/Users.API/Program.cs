using System.Reflection;
using Common.Logging;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Serilog;
using Users.API.Extensions;
using Users.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo
    {
        Description = "API for user authentication",
        Contact = new OpenApiContact
        {
            Email = "kostyabek@gmail.com",
            Name = "Kostiantyn Biektin"
        },
        Title = "Pandora's Box",
        Version = "1.0.0",
    });

    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
});

builder.Services
    .AddCookieAuthentication()
    .AddDataAccess(builder.Configuration)
    .AddConfigurations(builder.Configuration)
    .AddIdentityConfiguration()
    .AddMediatR(typeof(MediatREntryPoint).Assembly)
    .AddCustomServices()
    .AddFluentValidation(o => o.RegisterValidatorsFromAssemblyContaining(typeof(Program)))
    .AddHealthCheck();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        o.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    
    endpoints.MapHealthChecks("/hc", new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
});

app.Run();
