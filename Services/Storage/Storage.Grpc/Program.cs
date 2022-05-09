using Calzolari.Grpc.AspNetCore.Validation;
using Common.Logging;
using Serilog;
using Storage.Grpc.Extensions;
using Storage.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

builder.Services
    .AddDataAccess(builder.Configuration)
    .AddRabbitMQ(builder.Configuration)
    .ConfigureCustomSettings(builder.Configuration)
    .AddCustomServices()
    .AddFluentValidation()
    .AddGrpc(options => options.EnableMessageValidation())
    .AddServiceOptions<StorageGrpcService>(options => { options.MaxReceiveMessageSize = 100 * 1024 * 1024; });

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<StorageGrpcService>();

    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client.");
    });
});

app.Run();