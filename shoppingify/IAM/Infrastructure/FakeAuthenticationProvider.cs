using shoppingify.IAM.Application;

namespace shoppingify.IAM.Infrastructure;

class FakeAuthenticationProvider : IAuthenticationProviderService
{
    public Task<string> VerifyToken(string token)
    {
        return Task.FromResult("mock-user-id");
    }
}