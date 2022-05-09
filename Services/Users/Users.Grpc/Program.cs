using Common.Logging;
using Serilog;
using Users.Grpc.Extensions;
using Users.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

builder.Services
    .AddAutoMapper()
    .AddRepositories()
    .AddDataAccess(builder.Configuration)
    .AddCookieAuthentication()
    .AddGrpc();

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<UsersService>();

    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client.");
    });
});

app.Run();