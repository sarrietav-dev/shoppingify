using MediatR;
using shoppingify.IAM.Application.Exceptions;
using shoppingify.IAM.Domain;

namespace shoppingify.IAM.Application;

public class IdentityApplicationService
{
    private readonly IAuthenticationProviderService _authenticationProviderService;
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public IdentityApplicationService(IAuthenticationProviderService authenticationProviderService, IMediator mediator,
        ILogger<IdentityApplicationService> logger)
    {
        _authenticationProviderService = authenticationProviderService;
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Registers a user in the system. If the token is valid, a <see cref="UserCreatedEvent"/> is published.
    /// </summary>
    /// <param name="uid">
    /// The token to validate. The token must be in the format "Bearer {token}".
    /// </param>
    /// <exception cref="InvalidTokenException">
    /// Thrown when the token is invalid.
    /// </exception>
    public async Task RegisterUser(string uid)
    {
        var verifiedUid = await _authenticationProviderService.VerifyToken(uid);

        if (verifiedUid is null)
        {
            var exception = new InvalidTokenException("Invalid token");
            _logger.LogWarning(exception, "Invalid token for uid {Uid}", uid);
            throw exception;
        }

        _logger.LogInformation("User {Uid} validated and registered", uid);
        await _mediator.Publish(new UserCreatedEvent(verifiedUid));
    }
}