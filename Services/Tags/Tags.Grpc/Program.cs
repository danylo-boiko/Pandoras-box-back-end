using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Tags.Core.Extensions;
using Tags.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDataAccess(builder.Configuration)
    .AddRepositories()
    .AddMediatr()
    .AddAutoMapper()
    .AddFluentValidation()
    .AddUsersGrpcServer(builder.Configuration["GrpcSettings:UsersUrl"])
    .AddHealthCheck()
    .AddGrpc();

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<TagsService>();

    endpoints.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");
    
    endpoints.MapHealthChecks("/hc", new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
});

app.Run();