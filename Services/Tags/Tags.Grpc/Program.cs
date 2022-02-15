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
    .AddGrpc();

var app = builder.Build();

app.MapGrpcService<TagsService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();