using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Shoppingify.Products.Application;
using Shoppingify.Products.Application.Commands;

namespace Shoppingify.Products.Infrastructure.Controllers;

[ApiController]
[Route("api/v1/me/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductApplicationService _applicationService;

    public ProductsController(IProductApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var uid = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        if (uid is null) return BadRequest();

        var product = await _applicationService.Get(id);

        if (product is null)
            return NotFound();

        return Ok(product);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var id = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        if (id is null) return BadRequest();

        var products = await _applicationService.GetAll(id);

        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddProductCommand product)
    {
        var id = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        if (id is null) return BadRequest();

        var createdProduct = await _applicationService.Add(id, product);

        return CreatedAtAction(nameof(Add), new { id = createdProduct.Id }, product);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var uid = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        if (uid is null) return BadRequest();

        await _applicationService.Delete(id);

        return Ok();
    }
}