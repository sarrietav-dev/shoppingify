using Microsoft.AspNetCore.Mvc;
using shoppingify.Cart.Application;
using shoppingify.Cart.Domain;
using shoppingify.IAM.Application;

namespace shoppingify.Cart.Infrastructure.Controllers;

[ApiController]
[Route("api/v1/me")]
public class CartController : ControllerBase
{
    private readonly CartApplicationService _cartApplicationService;
    private readonly IAuthenticationProviderService _authenticationProviderService;

    public CartController(CartApplicationService cartApplicationService,
        IAuthenticationProviderService authenticationProviderService)
    {
        _cartApplicationService = cartApplicationService;
        _authenticationProviderService = authenticationProviderService;
    }

    [HttpGet("/carts")]
    public async Task<IActionResult> GetCarts()
    {
        var cartOwnerId = await GetAuthorizationToken();

        var carts = await _cartApplicationService.GetCarts(cartOwnerId);

        return Ok(carts);
    }

    [HttpGet("/active-cart")]
    public async Task<IActionResult> GetActiveCart()
    {
        var cartOwnerId = await GetAuthorizationToken();

        var cart = await _cartApplicationService.GetActiveCart(cartOwnerId);

        if (cart == null) return NotFound();

        return Ok(cart);
    }

    [HttpPost("/active-cart")]
    public async Task<IActionResult> CreateCart(string name)
    {
        try
        {
            var cartOwnerId = await GetAuthorizationToken();
            await _cartApplicationService.CreateCart(cartOwnerId, name);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return BadRequest();
        }
    }

    [HttpPut("/active-cart/items")]
    public async Task<IActionResult> UpdateCartList([FromBody] IEnumerable<CartItem> cartItems)
    {
        try
        {
            var ownerId = await GetAuthorizationToken();
            await _cartApplicationService.UpdateCartList(ownerId, cartItems);
            return Ok();
        }
        catch (InvalidOperationException)
        {
            return BadRequest();
        }
    }

    [HttpPut("/active-cart/complete")]
    public async Task<IActionResult> CompleteCart()
    {
        try
        {
            var cartOwnerId = await GetAuthorizationToken();
            await _cartApplicationService.CompleteCart(cartOwnerId);
            return Ok();
        }
        catch (InvalidOperationException)
        {
            return BadRequest();
        }
    }

    [HttpPut("/active-cart/cancel")]
    public async Task<IActionResult> CancelCart()
    {
        try
        {
            var cartOwnerId = await GetAuthorizationToken();
            await _cartApplicationService.CancelCart(cartOwnerId);
            return Ok();
        }
        catch (InvalidOperationException)
        {
            return BadRequest();
        }
    }

    private async Task<string> GetAuthorizationToken()
    {
        var uid = Request.Headers["Authorization"].ToString().Split(" ")[1];
        var cartOwnerId = await _authenticationProviderService.VerifyToken(uid);
        return cartOwnerId;
    }
}