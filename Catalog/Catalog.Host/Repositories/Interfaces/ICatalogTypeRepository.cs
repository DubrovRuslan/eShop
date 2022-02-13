using Catalog.Host.Data;
using Catalog.Host.Data.Entities;

namespace Catalog.Host.Repositories.Interfaces
{
    public interface ICatalogTypeRepository
    {
        Task<PaginatedItems<CatalogType>> GetByPageAsync(int pageIndex, int pageSize);
        Task<SelectedItems<CatalogType>> GetTypesAsync();
        Task<int?> AddAsync(string type);
        Task<bool?> UpdateAsync(int id, string type);
        Task<bool?> RemoveAsync(int id);
    }
}
