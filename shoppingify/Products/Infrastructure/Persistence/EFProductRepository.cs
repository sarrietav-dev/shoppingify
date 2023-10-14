using Microsoft.EntityFrameworkCore;
using Shoppingify.Products.Domain;

namespace Shoppingify.Products.Infrastructure.Persistence;

public class EfProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public EfProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task Add(Product product)
    {
        await _context.AddAsync(product);
    }

    public Task Delete(ProductId productId)
    {
        _context.Products.Where(x => x.Id == productId).ToList().ForEach(x => _context.Remove(x));
        return Task.CompletedTask;
    }

    public async Task<Product?> Get(ProductId id)
    {
        return await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Product>> GetAll(ProductOwner owner)
    {
        return await _context.Products.Where(x => x.Owner.Equals(owner)).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetAll()
    {
        return await _context.Products.ToListAsync();
    }
}