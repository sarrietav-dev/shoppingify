using FirebaseAdmin.Auth;
using Shoppingify.IAM.Application;

namespace Shoppingify.IAM.Infrastructure;

internal class FirebaseAuthenticationProvider : IAuthenticationProviderService
{
    public async Task<string> VerifyToken(string token)
    {
        var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);

        return decodedToken.Uid;
    }
}