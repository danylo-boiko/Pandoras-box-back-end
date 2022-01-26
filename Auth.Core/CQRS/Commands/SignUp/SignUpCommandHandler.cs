namespace Auth.Core.CQRS.Commands.SignUp;

using System.Threading;
using System.Threading.Tasks;
using Database;
using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.Extensions.Logging;

/// <summary>
/// SignUpCommand handler.
/// </summary>
/// <seealso cref="IRequestHandler{SignUpCommand}" />
public class SignUpCommandHandler : IRequestHandler<SignUpCommand, ExecutionResult>
{
    private readonly ILogger<SignUpCommandHandler> _logger;
    private readonly BaseDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="SignUpCommandHandler" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="dbContext">The database context.</param>
    public SignUpCommandHandler(
        ILogger<SignUpCommandHandler> logger,
        BaseDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    /// <summary>
    /// Handles the specified request.
    /// </summary>
    /// <param name="request">The request: SignUpCommand</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>string</returns>
    public async Task<ExecutionResult> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        return new ExecutionResult();
    }
}