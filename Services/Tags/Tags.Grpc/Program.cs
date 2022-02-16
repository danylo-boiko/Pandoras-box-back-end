using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Tags.Core.Extensions;
using Tags.Grpc.Extensions;
using Tags.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDataAccess(builder.Configuration)
    .AddRepositories()
    .AddMediatr()
    .AddAutoMapper()
    .AddUsersGrpc(builder.Configuration["GrpcSettings:UsersUrl"])
    .AddHealthCheck()
    .AddGrpc();

var app = builder.Build();

app.MapGrpcService<TagsService>();

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