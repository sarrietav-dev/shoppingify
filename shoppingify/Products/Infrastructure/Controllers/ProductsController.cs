using Microsoft.AspNetCore.Mvc;
using Shoppingify.Products.Application;

namespace Shoppingify.Products.Infrastructure.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductApplicationService _applicationService;

    public ProductsController(ProductApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var product = await _applicationService.Get(id);

        if (product is null)
            return NotFound();

        return Ok(product);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAll(Guid id)
    {
        var products = await _applicationService.GetAll(id);

        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddProductCommand product)
    {
        await _applicationService.Add(product);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _applicationService.Delete(id);

        return Ok();
    }

}