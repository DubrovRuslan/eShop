using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;

namespace Catalog.Host.Services.Interfaces;

public interface ICatalogService
{
    Task<PaginatedItemsResponse<CatalogItemDto>> GetCatalogItemsAsync(int pageSize, int pageIndex);
    Task<CatalogItemDto> GetCatalogItemByIdAsync(int id);
    Task<SelectedItemsResponse<CatalogItemDto>> GetCatalogItemByBrandAsync(string brand);
    Task<SelectedItemsResponse<CatalogItemDto>> GetCatalogItemByTypeAsync(string type);
}