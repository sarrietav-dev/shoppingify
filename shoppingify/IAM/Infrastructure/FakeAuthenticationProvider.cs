using Shoppingify.IAM.Application;

namespace Shoppingify.IAM.Infrastructure;

internal class FakeAuthenticationProvider : IAuthenticationProviderService
{
    public Task<string> VerifyToken(string token)
    {
        return Task.FromResult("mock-user-id");
    }
}