using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Shoppingify.IAM.Application;

public class AppAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
}

public class AppAuthenticationHandler : AuthenticationHandler<AppAuthenticationSchemeOptions>
{
    private readonly IAuthenticationProviderService _authenticationProviderService;
    public AppAuthenticationHandler(
        IOptionsMonitor<AppAuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock, IAuthenticationProviderService authenticationProviderService) : base(options, logger, encoder, clock)
    {
        _authenticationProviderService = authenticationProviderService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token == null)
        {
            return AuthenticateResult.NoResult();
        }

        var uid = await _authenticationProviderService.VerifyToken(token);

        if (uid == null)
        {
            return AuthenticateResult.Fail("Invalid token");
        }

        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, uid),
            new Claim(ClaimTypes.Name, uid)
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}