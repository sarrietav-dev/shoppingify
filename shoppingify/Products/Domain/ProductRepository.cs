namespace Shoppingify.Products.Domain;

public interface IProductRepository
{
    Task<Product> Get(ProductId id);
    Task<IEnumerable<Product>> GetAll(ProductOwner owner);
    Task Add(Product product);
    Task Delete(ProductId productId);
}