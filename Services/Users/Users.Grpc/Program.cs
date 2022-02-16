using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Users.Grpc.Extensions;
using Users.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDataAccess(builder.Configuration)
    .AddAutoMapper()
    .AddCustomRepositories()
    .AddHealthCheck()
    .AddGrpc();

var app = builder.Build();

app.MapGrpcService<UsersService>();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");
    endpoints.MapHealthChecks("/hc", new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
});

app.Run();