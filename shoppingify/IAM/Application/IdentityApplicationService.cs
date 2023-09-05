using MediatR;
using shoppingify.IAM.Domain;

namespace shoppingify.IAM.Application;

public class IdentityApplicationService
{
    private readonly IAuthenticationProviderService _authenticationProviderService;
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public IdentityApplicationService(IAuthenticationProviderService authenticationProviderService, IMediator mediator, ILogger logger)
    {
        _authenticationProviderService = authenticationProviderService;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task RegisterUser(string uid)
    {
        var verifiedUid = await _authenticationProviderService.VerifyToken(uid);

        if (verifiedUid is null)
        {
            var exception = new Exception("Invalid token");
            _logger.LogWarning(exception, "Invalid token for uid {Uid}", uid);
            throw exception;
        }
        
        _logger.LogInformation("User {Uid} validated and registered", uid);
        await _mediator.Publish(new UserCreatedEvent(verifiedUid));
    }
}