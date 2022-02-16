using System.Net;
using Catalog.Host.Services.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Response;

namespace Catalog.Host.Controllers;

[ApiController]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogTypeController : ControllerBase
{
    private readonly ILogger<CatalogTypeController> _logger;
    private readonly ICatalogTypeService _catalogTypeService;

    public CatalogTypeController(
        ILogger<CatalogTypeController> logger,
        ICatalogTypeService catalogTypeService)
    {
        _logger = logger;
        _catalogTypeService = catalogTypeService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddTypeResponse<int?>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Add(CreateTypeRequest request)
    {
        var result = await _catalogTypeService.Add(request.Type);
        return Ok(new AddTypeResponse<int?>() { Id = result });
    }

    [HttpPost]
    [ProducesResponseType(typeof(ChangeTypeResponse<bool?>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(UpdateTypeRequest request)
    {
        var result = await _catalogTypeService.Update(request.Id, request.Type);
        return Ok(new ChangeTypeResponse<bool?>() { Result = result });
    }

    [HttpPost]
    [ProducesResponseType(typeof(ChangeItemResponse<bool?>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Remove(UpdateTypeRequest request)
    {
        var result = await _catalogTypeService.Remove(request.Id);
        return Ok(new ChangeTypeResponse<bool?>() { Result = result });
    }
}