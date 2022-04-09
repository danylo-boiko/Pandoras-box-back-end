﻿using Users.Core.Enums;

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
    private readonly IOptions<StorageServiceOptions> _storageServiceOptions;

    public SetAvatarCommandHandler(IOptions<StorageServiceOptions> storageServiceOptions)
    {
        _storageServiceOptions = storageServiceOptions;
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
            var grpcRequest = new SaveMediaFilesRequest
            {
                FileBytes = ByteString.CopyFrom(avatarBytes),
                CategoryId = (int)FileCategory.Avatar,
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
            return new ExecutionResult(new ErrorInfo($"Error while setting a new avatar. {e.Message}"));
        }
    }
}