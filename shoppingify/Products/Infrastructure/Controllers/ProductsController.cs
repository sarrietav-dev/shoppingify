using Microsoft.AspNetCore.Mvc;
using shoppingify.Products.Application;

namespace shoppingify.Products.Infrastructure.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductApplicationService _applicationService;

    public ProductsController(ProductApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var product = await _applicationService.Get(id);

        if (product is null)
            return NotFound();

        return Ok(product);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(string id)
    {
        var products = await _applicationService.GetAll(id);

        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> Add(string id, AddProductCommand product)
    {
        await _applicationService.Add(id, product);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _applicationService.Delete(id);

        return Ok();
    }
}