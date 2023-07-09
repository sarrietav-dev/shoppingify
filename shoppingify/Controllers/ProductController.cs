using Microsoft.AspNetCore.Mvc;
using shoppingify.Entities;
using shoppingify.Services;

namespace shoppingify.Controllers;

[ApiController]
[Route("[controller]")]
class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }
    
    [HttpGet(Name = "GetProducts")]
    public IEnumerable<Product> Get()
    {
        return _productService.GetProductsAsync().ToList();
    }
    
    [HttpGet("{id}", Name = "GetProductById")]
    public async Task<Product> Get(int id)
    {
        return await _productService.GetProductByIdAsync(id);
    }
    
    [HttpPost(Name = "CreateProduct")]
    public async Task<Product> Post([FromBody] Product product)
    {
        return await _productService.CreateProductAsync(product);
    }
}