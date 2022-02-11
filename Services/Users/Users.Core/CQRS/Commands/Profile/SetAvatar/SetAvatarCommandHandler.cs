namespace Users.Core.CQRS.Commands.Profile.SetAvatar;

using System.Threading;
using System.Threading.Tasks;
using Configurations;
using Consts;
using Extensions;
using Google.Protobuf;
using Grpc.Net.Client;
using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// SetAvatarCommand handler.
/// </summary>
/// <seealso cref="IRequestHandler{SetAvatarCommand}" />
public class SetAvatarCommandHandler : IRequestHandler<SetAvatarCommand, ExecutionResult>
{
    private readonly ILogger<SetAvatarCommandHandler> _logger;
    private readonly IOptions<StorageServiceOptions> _storageServiceOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="SetAvatarCommandHandler" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="storageServiceOptions"></param>
    public SetAvatarCommandHandler(
        ILogger<SetAvatarCommandHandler> logger,
        IOptions<StorageServiceOptions> storageServiceOptions)
    {
        _logger = logger;
        _storageServiceOptions = storageServiceOptions;
    }

    /// <summary>
    /// Handles the specified request.
    /// </summary>
    /// <param name="request">The request: SetAvatarCommand</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>string</returns>
    public async Task<ExecutionResult> Handle(SetAvatarCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var storageServiceAddress = _storageServiceOptions.Value.Address;

            var grpcChannel = GrpcChannel.ForAddress(storageServiceAddress);
            var grpcClient = new Storage.StorageClient(grpcChannel);

            var avatarBytes = await request.Avatar.GetBytesAsync();
            var fileExtension = Path.GetExtension(request.Avatar.FileName);
            var grpcRequest = new SaveMediaFilesRequest
            {
                FileBytes = ByteString.CopyFrom(avatarBytes),
                CategoryId = (int)FileCategories.Avatar,
                Extension = fileExtension
            };

            using var call = grpcClient.SaveMediaFiles();
            await call.RequestStream.WriteAsync(grpcRequest);
            await call.RequestStream.CompleteAsync();

            var response = call.ResponseAsync.Result;

            return response.IsSuccess
                ? new ExecutionResult(new InfoMessage("Avatar has been set successfully."))
                : new ExecutionResult(new ErrorInfo(response.Message));
        }
        catch (Exception e)
        {
            return new ExecutionResult(new ErrorInfo("Error while setting a new avatar."));
        }
    }
}