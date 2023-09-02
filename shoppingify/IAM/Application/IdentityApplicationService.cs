using MediatR;
using shoppingify.IAM.Domain;

namespace shoppingify.IAM.Application;

public class IdentityApplicationService
{
    private IAuthenticationProviderService _authenticationProviderService;
    private IMediator _mediator;

    public IdentityApplicationService(IAuthenticationProviderService authenticationProviderService, IMediator mediator)
    {
        _authenticationProviderService = authenticationProviderService;
        _mediator = mediator;
    }

    public async Task RegisterUser(string uid)
    {
        var verifiedUid = await _authenticationProviderService.VerifyToken(uid);

        if (verifiedUid is null)
        {
            throw new Exception("Invalid token");
        }
        
        await _mediator.Publish(new UserCreatedEvent(verifiedUid));
    }
}