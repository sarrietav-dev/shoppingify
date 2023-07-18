using Microsoft.AspNetCore.Mvc;
using shoppingify.Services;

namespace shoppingify.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShoppingCartController : ControllerBase
{
    private readonly IShoppingCartService _shoppingCartService;

    public ShoppingCartController(IShoppingCartService shoppingCartService)
    {
        _shoppingCartService = shoppingCartService;
    }
    
    [HttpPatch(Name = "AddItemToCart")]
    public async Task AddItemToCart([FromBody] AddItemToCartInput input)
    {
        await _shoppingCartService.AddItemToCart(input.CartId, input.ProductId);
    }
    
    [HttpDelete(Name = "RemoveItemFromCart")]
    public async Task RemoveItemFromCart([FromBody] RemoveItemFromCartInput input)
    {
        await _shoppingCartService.RemoveItemFromCart(input.CartId, input.ProductId);
    }
    
    [HttpPatch("items", Name = "EditItemsCount")]
    public async Task EditItemsCount([FromBody] EditItemsCountInput items)
    {
        await _shoppingCartService.EditItemsCount(items.CartId, items.Items);
    }
    
    [HttpPatch("items/check", Name = "CheckItem")]
    public async Task CheckItem([FromBody] AddItemToCartInput input)
    {
        await _shoppingCartService.CheckItem(input.CartId, input.ProductId);
    }
    
    [HttpPatch("items/uncheck", Name = "UncheckItem")]
    public async Task UncheckItem([FromBody] RemoveItemFromCartInput input)
    {
        await _shoppingCartService.UncheckItem(input.CartId, input.ProductId);
    }
    
    [HttpPost("save", Name = "SaveCart")]
    public void SaveCart([FromBody] SaveCartInput cart)
    {
        _shoppingCartService.SaveCart(cart);
    }
}

public record EditItemsCountInput(string CartId, IEnumerable<SetItemCountInput> Items);

public abstract record AddItemToCartInput(string CartId, string ProductId);
public abstract record RemoveItemFromCartInput(string CartId, string ProductId);