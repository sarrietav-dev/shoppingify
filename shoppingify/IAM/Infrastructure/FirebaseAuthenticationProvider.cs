using FirebaseAdmin.Auth;
using shoppingify.IAM.Application;

namespace shoppingify.IAM.Infrastructure;

class FirebaseAuthenticationProvider : IAuthenticationProviderService
{
    public async Task<string> VerifyToken(string token)
    {
        var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);

        return decodedToken.Uid;
    }
}