using Microsoft.Extensions.Logging;
using Users.Core.Enums;
using Users.Core.Services.User;

namespace Users.Core.CQRS.Commands.Profile.SetAvatar;

using System.Threading;
using System.Threading.Tasks;
using Configurations;
using Extensions;
using Google.Protobuf;
using Grpc.Net.Client;
using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.Extensions.Options;

public class SetAvatarCommandHandler : IRequestHandler<SetAvatarCommand, ExecutionResult>
{
    private readonly ILogger<SetAvatarCommandHandler> _logger;
    private readonly IOptions<StorageServiceOptions> _storageServiceOptions;
    private readonly IUserService _userService;

    public SetAvatarCommandHandler(
        ILogger<SetAvatarCommandHandler> logger,
        IOptions<StorageServiceOptions> storageServiceOptions,
        IUserService userService)
    {
        _logger = logger;
        _storageServiceOptions = storageServiceOptions;
        _userService = userService;
    }

    public async Task<ExecutionResult> Handle(SetAvatarCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var storageServiceAddress = _storageServiceOptions.Value.Address;

            var grpcChannel = GrpcChannel.ForAddress(storageServiceAddress);
            var grpcClient = new Storage.StorageClient(grpcChannel);

            var avatarBytes = await request.Avatar.GetBytesAsync();
            var fileExtension = Path.GetExtension(request.Avatar.FileName);

            var userId = _userService.UserId;
            var grpcRequest = new SaveMediaFilesRequest
            {
                FileBytes = ByteString.CopyFrom(avatarBytes),
                CategoryId = (int)FileCategory.Avatar,
                Extension = fileExtension,
                UserId = userId
            };

            using var call = grpcClient.SaveMediaFiles();
            await call.RequestStream.WriteAsync(grpcRequest);
            await call.RequestStream.CompleteAsync();

            var response = call.ResponseAsync.Result;

            if (response.IsSuccess)
            {
                _logger.LogInformation("Avatar for user with id: {Id} has been set successfully", userId);
                return new ExecutionResult(new InfoMessage($"Avatar for user with id: {userId}  has been set successfully."));
            }
            return new ExecutionResult(new ErrorInfo(response.Message));
        }
        catch (Exception e)
        {
            return new ExecutionResult(new ErrorInfo($"Error while setting a new avatar. {e.Message}"));
        }
    }
}