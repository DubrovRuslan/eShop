using System.Net;
using Catalog.Host.Services.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Response;

namespace Catalog.Host.Controllers;

[ApiController]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogBrandController : ControllerBase
{
    private readonly ILogger<CatalogBrandController> _logger;
    private readonly ICatalogBrandService _catalogBrandService;

    public CatalogBrandController(
        ILogger<CatalogBrandController> logger,
        ICatalogBrandService catalogBrandService)
    {
        _logger = logger;
        _catalogBrandService = catalogBrandService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddBrandResponse<int?>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Add(CreateBrandRequest request)
    {
        var result = await _catalogBrandService.Add(request.Brand);
        return Ok(new AddBrandResponse<int?>() { Id = result });
    }

    [HttpPost]
    [ProducesResponseType(typeof(ChangeBrandResponse<bool?>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(UpdateBrandRequest request)
    {
        var result = await _catalogBrandService.Update(request.Id, request.Brand);
        return Ok(new ChangeBrandResponse<bool?>() { Result = result });
    }

    [HttpPost]
    [ProducesResponseType(typeof(ChangeBrandResponse<bool?>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Remove(UpdateBrandRequest request)
    {
        var result = await _catalogBrandService.Remove(request.Id);
        return Ok(new ChangeBrandResponse<bool?>() { Result = result });
    }
}