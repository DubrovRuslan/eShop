using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories
{
    public class CatalogTypeRepository : ICatalogTypeRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CatalogTypeRepository> _logger;

        public CatalogTypeRepository(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<CatalogTypeRepository> logger)
        {
            _dbContext = dbContextWrapper.DbContext;
            _logger = logger;
        }

        public async Task<int?> AddAsync(string type)
        {
            var item = await _dbContext.AddAsync(new CatalogType
            {
                Type = type
            });

            await _dbContext.SaveChangesAsync();

            return item.Entity.Id;
        }

        public async Task<bool?> RemoveAsync(int id)
        {
            var item = await _dbContext.CatalogTypes.Where(i => i.Id == id).FirstAsync();
            if (item is not null)
            {
                _dbContext.CatalogTypes.Remove(item);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<PaginatedItems<CatalogType>> GetByPageAsync(int pageIndex, int pageSize)
        {
            var totalItems = await _dbContext.CatalogItems
            .LongCountAsync();

            var itemsOnPage = await _dbContext.CatalogTypes
                .OrderBy(c => c.Type)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedItems<CatalogType>() { TotalCount = totalItems, Data = itemsOnPage };
        }

        public async Task<bool?> UpdateAsync(int id, string type)
        {
            var item = await _dbContext.CatalogTypes.Where(i => i.Id == id).FirstAsync();
            if (item is not null)
            {
                item.Type = type;
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<SelectedItems<CatalogType>> GetTypesAsync()
        {
            var result = await _dbContext.CatalogTypes.Distinct<CatalogType>().ToListAsync();
            return new SelectedItems<CatalogType>() { TotalCount = result.Count, Data = result };
        }
    }
}
