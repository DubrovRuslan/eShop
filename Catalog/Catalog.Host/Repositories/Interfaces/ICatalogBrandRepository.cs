using Catalog.Host.Data;
using Catalog.Host.Data.Entities;

namespace Catalog.Host.Repositories.Interfaces
{
    public interface ICatalogBrandRepository
    {
        Task<PaginatedItems<CatalogBrand>> GetByPageAsync(int pageIndex, int pageSize);
        Task<SelectedItems<CatalogBrand>> GetBrandsAsync();
        Task<int?> AddAsync(string brand);
        Task<bool?> UpdateAsync(int id, string brand);
        Task<bool?> RemoveAsync(int id);
    }
}
