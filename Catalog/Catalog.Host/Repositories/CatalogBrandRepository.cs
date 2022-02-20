using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;

namespace Catalog.Host.Repositories
{
    public class CatalogBrandRepository : ICatalogBrandRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CatalogBrandRepository> _logger;

        public CatalogBrandRepository(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<CatalogBrandRepository> logger)
        {
            _dbContext = dbContextWrapper.DbContext;
            _logger = logger;
        }

        public async Task<PaginatedItems<CatalogBrand>> GetByPageAsync(int pageIndex, int pageSize)
        {
            var totalItems = await _dbContext.CatalogBrands
                .LongCountAsync();

            var itemsOnPage = await _dbContext.CatalogBrands
                .OrderBy(c => c.Brand)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedItems<CatalogBrand>() { TotalCount = totalItems, Data = itemsOnPage };
        }

        public async Task<int?> AddAsync(string brand)
        {
            var item = await _dbContext.AddAsync(new CatalogBrand
            {
                Brand = brand
            });

            await _dbContext.SaveChangesAsync();

            return item.Entity.Id;
        }

        public async Task<bool?> UpdateAsync(int id, string brand)
        {
            var item = await _dbContext.CatalogBrands.Where(i => i.Id == id).FirstAsync();
            if (item is not null)
            {
                item.Brand = brand;
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool?> RemoveAsync(int id)
        {
            var item = await _dbContext.CatalogBrands.Where(i => i.Id == id).FirstAsync();
            if (item is not null)
            {
                _dbContext.CatalogBrands.Remove(item);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<SelectedItems<CatalogBrand>> GetBrandsAsync()
        {
            var result = await _dbContext.CatalogBrands.Distinct<CatalogBrand>().ToListAsync();
            return new SelectedItems<CatalogBrand>() { TotalCount = result.Count, Data = result };
        }
    }
}
