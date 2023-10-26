using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Shoppingify.Cart.Application;
using Shoppingify.Cart.Application.DTOs;
using Shoppingify.Cart.Domain;

namespace Shoppingify.Cart.Infrastructure.Controllers;

[ApiController]
[Route("api/v1/me")]
public class CartController : ControllerBase
{
    private readonly ICartApplicationService _cartApplicationService;

    public CartController(ICartApplicationService cartApplicationService)
    {
        _cartApplicationService = cartApplicationService;
    }

    [HttpGet("carts")]
    public async Task<IActionResult> GetCarts()
    {
        var cartOwnerId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        if (cartOwnerId == null) return BadRequest();

        var carts = await _cartApplicationService.GetCarts(cartOwnerId);

        return Ok(carts);
    }

    [HttpGet("active-cart")]
    public async Task<IActionResult> GetActiveCart()
    {
        var cartOwnerId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        if (cartOwnerId == null) return BadRequest();

        var cart = await _cartApplicationService.GetActiveCart(cartOwnerId);

        if (cart == null) return NoContent();
        
        return Ok(cart);
    }

    [HttpPost("active-cart")]
    public async Task<IActionResult> CreateCart([FromBody] CreateCartCommand cart)
    {
        var cartOwnerId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        if (cartOwnerId == null) return BadRequest();

        try
        {
            var cartId = await _cartApplicationService.CreateCart(cartOwnerId, cart.Name, cart.CartItems);

            if (cartId == null) return StatusCode(500);

            return CreatedAtAction(nameof(GetActiveCart), new { cartOwnerId }, new { id = cartId.Value });
        }
        catch (InvalidOperationException)
        {
            return BadRequest(new { Message = "Cart already has an active cart" });
        }
    }

    [HttpPut("active-cart/items")]
    public async Task<IActionResult> UpdateCartList([FromBody] IEnumerable<CartItemDtoWithoutProduct> cartItems)
    {
        var ownerId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        if (ownerId == null) return BadRequest();

        try
        {
            await _cartApplicationService.UpdateCartList(ownerId, cartItems);
            return Ok();
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(new { e.Message });
        }
    }

    [HttpPut("active-cart/complete")]
    public async Task<IActionResult> CompleteCart()
    {
        var cartOwnerId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        if (cartOwnerId == null) return BadRequest();
        try
        {
            await _cartApplicationService.CompleteCart(cartOwnerId);
            return Ok();
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(new { e.Message });
        }
    }

    [HttpPut("active-cart/cancel")]
    public async Task<IActionResult> CancelCart()
    {
        var cartOwnerId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        if (cartOwnerId == null) return BadRequest();
        try
        {
            await _cartApplicationService.CancelCart(cartOwnerId);
            return Ok();
        }
        catch (InvalidOperationException)
        {
            return BadRequest();
        }
    }
}

public record CreateCartCommand(string Name, IEnumerable<CartItemDtoWithoutProduct> CartItems);