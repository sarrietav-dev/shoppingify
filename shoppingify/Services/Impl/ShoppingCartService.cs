using shoppingify.Entities;
using shoppingify.Repositories;

namespace shoppingify.Services.Impl;

public class ShoppingCartService : IShoppingCartService
{
    private readonly IShoppingCartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public ShoppingCartService(IShoppingCartRepository cartRepository, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }
    
    public async Task AddItemToCart(string cartId, string productId)
    {
        var cart = _cartRepository.GetCart(cartId);
        var product = await _productRepository.GetProductById(productId);
        
        cart.AddItem(product);
        
        _cartRepository.SaveCart();
    }

    public async Task RemoveItemFromCart(string cartId, string productId)
    {
        var cart = _cartRepository.GetCart(cartId);
        var product = await _productRepository.GetProductById(productId);
        
        cart.RemoveItem(product);
        
        _cartRepository.SaveCart();
    }

    public async Task EditItemsCount(string cartId, IEnumerable<SetItemCountInput> items)
    {
        var cart = _cartRepository.GetCart(cartId);
        foreach (var item in items)
        {
            var product = await _productRepository.GetProductById(item.ProductId);
            cart.ChangeItemQuantity(product, item.Count);
        }
        _cartRepository.SaveCart();
    }

    public async Task CheckItem(string cartId, string productId)
    {
        var cart = _cartRepository.GetCart(cartId);
        var product = await _productRepository.GetProductById(productId);
        
        cart.CheckItem(product);
        
        _cartRepository.SaveCart();
    }

    public async Task UncheckItem(string cartId, string productId)
    {
        var cart = _cartRepository.GetCart(cartId);
        var product = await _productRepository.GetProductById(productId);
        
        cart.UncheckItem(product);
        
        _cartRepository.SaveCart();
    }

    public void SaveCart(SaveCartInput cart)
    {
        var newCart = new ShoppingCart
        {
            Name = cart.CartName
        };
        var products = _productRepository.GetProductsById(cart.Items.Select(i => i.ProductId)).ToList();
        
        foreach (var item in cart.Items)
        {
            var product = products.First(p => p.Id == Guid.Parse(item.ProductId));
            newCart.AddItem(product, item.Count);
        }

        _cartRepository.CreateCart(newCart);
    }

    public Task<ShoppingCart> GetCart(string cartId)
    {
        return Task.FromResult(_cartRepository.GetCart(cartId));
    }
}