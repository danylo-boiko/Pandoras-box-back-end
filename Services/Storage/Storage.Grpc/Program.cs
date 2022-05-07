using Storage.Grpc.Extensions;
using Storage.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDataAccess(builder.Configuration)
    .AddRabbitMQ(builder.Configuration)
    .ConfigureCustomSettings(builder.Configuration)
    .AddCustomServices()
    .AddGrpc()
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