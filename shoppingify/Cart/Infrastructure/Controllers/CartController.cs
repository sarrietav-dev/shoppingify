using Microsoft.AspNetCore.Mvc;
using shoppingify.Cart.Application;
using shoppingify.Cart.Domain;

namespace shoppingify.Cart.Infrastructure.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly CartApplicationService _cartApplicationService;

    public CartController(CartApplicationService cartApplicationService)
    {
        _cartApplicationService = cartApplicationService;
    }
    
    [HttpGet("{cartOwnerId}")]
    public async Task<IActionResult> GetActiveCart(string cartOwnerId)
    {
        var cart = await _cartApplicationService.GetActiveCart(cartOwnerId);
        return Ok(cart);
    }
    
    [HttpPut("{cartId:guid}")]
    public async Task<IActionResult> UpdateCartList(Guid cartId, IEnumerable<CartItem> cartItems)
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