
using shoppingify.IAM.Application;

namespace shoppingify.Middleware;

public class AuthTokenMiddleware : IMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IAuthenticationProviderService _tokenService;

    public AuthTokenMiddleware(RequestDelegate next, IAuthenticationProviderService tokenService)
    {
        _next = next;
        _tokenService = tokenService;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            await AttachUserToContext(context, token);
        }
        else
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
        }

        await _next(context);
    }

    private async Task AttachUserToContext(HttpContext context, string token)
    {
        var uid = await _tokenService.VerifyToken(token);

        if (uid != null)
        {
            context.Request.Headers.Append("uid", uid);
        }
    }
}