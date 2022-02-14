using Users.Grpc.Extensions;
using Users.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDataAccess(builder.Configuration)
    .AddAutoMapper()
    .AddCustomRepositories()
    .AddGrpc();

var app = builder.Build();

app.MapGrpcService<UsersService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();