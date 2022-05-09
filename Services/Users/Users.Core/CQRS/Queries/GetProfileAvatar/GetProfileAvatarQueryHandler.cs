using Grpc.Net.Client;
using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.Extensions.Options;
using Users.Core.Configurations;
using Users.Core.Services.User;

namespace Users.Core.CQRS.Queries.GetProfileAvatar;

public class GetProfileAvatarQueryHandler : IRequestHandler<GetProfileAvatarQuery, ExecutionResult<GetProfileAvatarQueryResult>> 
{
    private readonly IUserService _userService;
    private readonly IOptions<StorageServiceOptions> _storageServiceOptions;

    public GetProfileAvatarQueryHandler(IUserService userService, IOptions<StorageServiceOptions> storageServiceOptions)
    {
        _userService = userService;
        _storageServiceOptions = storageServiceOptions;
    }

    public async Task<ExecutionResult<GetProfileAvatarQueryResult>> Handle(GetProfileAvatarQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _userService.UserId;

            var storageServiceAddress = _storageServiceOptions.Value.Address;

            var grpcChannel = GrpcChannel.ForAddress(storageServiceAddress);
            var grpcClient = new Storage.StorageClient(grpcChannel);

            var grpcRequest = new GetUserCurrentAvatarDataRequest
            {
                UserId = userId
            };

            var grpcResponse = await grpcClient.GetUserCurrentAvatarDataAsync(grpcRequest);

            var extensionWithoutDot = grpcResponse.Extension[1..];
            var result = new GetProfileAvatarQueryResult
            {
                AvatarStream = new MemoryStream(grpcResponse.AvatarBytes.ToByteArray()),
                Extension = extensionWithoutDot
            };

            return new ExecutionResult<GetProfileAvatarQueryResult>(result);
        }
        catch (Exception e)
        {
            return new ExecutionResult<GetProfileAvatarQueryResult>(new ErrorInfo($"Error while executing GetProfileAvatarQuery.\n> {e.Message}"));
        }
    }
}