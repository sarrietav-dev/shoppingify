using shoppingify.Entities;

namespace shoppingify.Repositories.Impl;

public class ProductRepository : IProductRepository
{
    private readonly ShoppingContext _context;

    public ProductRepository(ShoppingContext context)
    {
        _context = context;
    }

    public async Task<Product> GetProductById(string id)
    {
        return await _context.Products.FindAsync(Guid.Parse(id)) ?? throw new KeyNotFoundException();
    }

    public IEnumerable<Product> GetProductsById(IEnumerable<string> ids)
    {
        return _context.Products.Where(p => ids.Contains(p.Id.ToString()));
    }

    public IEnumerable<Product> GetAllProducts()
    {
        return _context.Products;
    }

    public Task CreateProductAsync(Product newProduct)
    {
        _context.Products.Add(newProduct);
        return _context.SaveChangesAsync();
    }

    public async Task DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return;
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}