using shoppingify.Products.Domain;

namespace shoppingify.Products.Application;

public interface IProductApplicationService
{
    Task<Product?> Get(Guid productId);
    Task<IEnumerable<Product>> GetAll(string productOwnerId);
    Task Add(string ownerId, AddProductCommand product);
    Task Delete(Guid productId);
}