using Microsoft.AspNetCore.Mvc;
using shoppingify.Entities;
using shoppingify.Services;

namespace shoppingify.Controllers;

[Route("api/[controller]")]
public class ProductsController : ControllerBase
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
    public async Task<Product> Get(string id)
    {
        return await _productService.GetProductByIdAsync(id);
    }
    
    [HttpPost(Name = "CreateProduct")]
    public async Task<Product> Post([FromBody] ProductInput product)
    {
        return await _productService.CreateProductAsync(product);
    }

    [HttpGet("categories", Name = "GetCategories")]
    public ActionResult<IEnumerable<string>> GetCategories()
    {
        return Ok(_productService.GetCategories());
    }
}
