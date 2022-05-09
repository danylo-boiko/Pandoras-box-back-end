using Common.Logging;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using Videos.API.Extensions;
using Videos.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 100 * 1024 * 1024;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddDataAccess(builder.Configuration)
    .AddMediatr()
    .AddFluentValidationValidators()
    .AddRabbitMQ(builder.Configuration)
    .AddStorageGrpc(builder.Configuration)
    .AddUsersGrpc(builder.Configuration)
    .AddTagsGrpc(builder.Configuration)
    .AddNsfwGrpc(builder.Configuration)
    .AddHealthCheck();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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