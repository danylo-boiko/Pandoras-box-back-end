using Users.Grpc.Extensions;
using Users.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddDataAccess(builder.Configuration);

var app = builder.Build();

app.MapGrpcService<UsersService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();