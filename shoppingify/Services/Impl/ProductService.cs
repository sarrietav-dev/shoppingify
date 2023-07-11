using shoppingify.Entities;

namespace shoppingify.Services.Impl;

class ProductService : IProductService
{
    private readonly ShoppingContext _context;
    
    public ProductService(ShoppingContext context)
    {
        _context = context;
    }
    
    public IEnumerable<Product> GetProductsAsync()
    {
        return _context.Products;
    }

    public async Task<Product> GetProductByIdAsync(string id)
    {
        return await _context.Products.FindAsync(Guid.Parse(id)) ?? throw new KeyNotFoundException();
    }

    public async Task<Product> CreateProductAsync(ProductInput product)
    {
        var newProduct = new Product
        {
            Name = product.Name,
            Note = product.Note,
            Category = product.Category,
            Image = product.Image
        };
        _context.Products.Add(newProduct);
        await _context.SaveChangesAsync();
        return newProduct;
    }

    public async Task DeleteProductAsync(int id)
    {
        _context
            .Products.Remove(await _context.Products.FindAsync(id) ?? throw new KeyNotFoundException());
        await _context.SaveChangesAsync();
    }
}