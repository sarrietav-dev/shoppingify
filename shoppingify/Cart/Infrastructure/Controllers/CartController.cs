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

    public CartController(CartApplicationService cartApplicationService, IAuthenticationProviderService authenticationProviderService)
    {
        _cartApplicationService = cartApplicationService;
        _authenticationProviderService = authenticationProviderService;
    }
    
    [HttpGet("/active-cart")]
    public async Task<IActionResult> GetActiveCart()
    {
        var uid = Request.Headers["Authorization"].ToString().Split(" ")[1];
        var cartOwnerId = await _authenticationProviderService.VerifyToken(uid);
        
        var cart = await _cartApplicationService.GetActiveCart(cartOwnerId);
        
        if (cart == null) return NotFound();
        
        return Ok(cart);
    }
    
    [HttpPut("/active-cart/items")]
    public async Task<IActionResult> UpdateCartList([FromBody] IEnumerable<CartItem> cartItems)
    {
        await _cartApplicationService.UpdateCartList(cartId, cartItems);
        return Ok();
    }
    
    [HttpPut("{cartId:guid}/complete")]
    public async Task<IActionResult> CompleteCart(Guid cartId)
    {
        await _cartApplicationService.CompleteCart(cartId);
        return Ok();
    }
    
    [HttpPut("{cartId:guid}/cancel")]
    public async Task<IActionResult> CancelCart(Guid cartId)
    {
        await _cartApplicationService.CancelCart(cartId);
        return Ok();
    }
    
    [HttpPost("{cartOwnerId}")]
    public async Task<IActionResult> CreateCart(string cartOwnerId, string name)
    {
        await _cartApplicationService.CreateCart(cartOwnerId, name);
        return Ok();
    }
}