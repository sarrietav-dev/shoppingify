using Microsoft.EntityFrameworkCore;
using shoppingify.Products.Domain;

namespace shoppingify.Products.Infrastructure.Persistence;

class EFProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public EFProductRepository(AppDbContext context)
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

    public async Task<IEnumerable<Product>> GetAll()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetAll(ProductOwner owner)
    {
        return await _context.Products.Where(x => x.Owner.Value == owner.Value).ToListAsync();
    }

    public Task Update(Product product)
    {
        _context.Update(product);
        return Task.CompletedTask;
    }
}