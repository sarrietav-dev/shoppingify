using shoppingify.IAM.Application;

namespace shoppingify.IAM.Infrastructure;

public class MockAuthenticationService : IAuthenticationProviderService
{
    public Task<string> VerifyToken(string token)
    {
        return token == "validToken" ? Task.FromResult("validUid") : Task.FromResult<string?>(null)!;
    }
}