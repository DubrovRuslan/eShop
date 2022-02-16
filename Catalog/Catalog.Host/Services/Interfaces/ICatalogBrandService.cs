using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;

namespace Catalog.Host.Services.Interfaces
{
    public interface ICatalogBrandService
    {
        Task<int?> Add(string brand);
        Task<bool?> Update(int id, string brand);
        Task<bool?> Remove(int id);
        Task<SelectedItemsResponse<CatalogBrandDto>> GetCatalogBrandsAsync();
    }
}
