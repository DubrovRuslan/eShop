using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;

namespace Catalog.Host.Services.Interfaces;
public interface ICatalogTypeService
{
    Task<int?> Add(string type);
    Task<bool?> Update(int id, string type);
    Task<bool?> Remove(int id);
    Task<SelectedItemsResponse<CatalogTypeDto>> GetCatalogTypesAsync();
}
