using Catalog.Host.Configurations;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Enums;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Response;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Controllers;

[ApiController]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogBffController : ControllerBase
{
    private readonly ILogger<CatalogBffController> _logger;
    private readonly ICatalogService _catalogService;
    private readonly ICatalogItemService _catalogItemService;
    private readonly ICatalogBrandService _catalogBrandService;
    private readonly ICatalogTypeService _catalogTypeService;

    public CatalogBffController(
        ILogger<CatalogBffController> logger,
        ICatalogService catalogService,
        ICatalogItemService catalogItemService,
        ICatalogBrandService catalogBrandService,
        ICatalogTypeService catalogTypeService)
    {
        _logger = logger;
        _catalogService = catalogService;
        _catalogItemService = catalogItemService;
        _catalogBrandService = catalogBrandService;
        _catalogTypeService = catalogTypeService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(PaginatedItemsResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Items(PaginatedItemsRequest<CatalogTypeFilter> request)
    {
        var result = await _catalogService.GetCatalogItemsAsync(request.PageSize, request.PageIndex, request.Filters);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CatalogItemDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetById(int request)
    {
        var result = await _catalogService.GetCatalogItemByIdAsync(request);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(SelectedItemsResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetByBrand(string request)
    {
        var result = await _catalogService.GetCatalogItemByBrandAsync(request);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(SelectedItemsResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetByType(string request)
    {
        var result = await _catalogService.GetCatalogItemByTypeAsync(request);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(SelectedItemsResponse<CatalogBrandDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetBrands()
    {
        var result = await _catalogBrandService.GetCatalogBrandsAsync();
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(SelectedItemsResponse<CatalogTypeDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetTypes()
    {
        var result = await _catalogTypeService.GetCatalogTypesAsync();
        return Ok(result);
    }
}