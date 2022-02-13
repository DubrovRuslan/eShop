using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories;

public class CatalogItemRepository : ICatalogItemRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CatalogItemRepository> _logger;

    public CatalogItemRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogItemRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize)
    {
        var totalItems = await _dbContext.CatalogItems
            .LongCountAsync();

        var itemsOnPage = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .OrderBy(c => c.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage };
    }

    public async Task<int?> AddAsync(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
    {
        var item = await _dbContext.AddAsync(new CatalogItem
        {
            CatalogBrandId = catalogBrandId,
            CatalogTypeId = catalogTypeId,
            Description = description,
            Name = name,
            PictureFileName = pictureFileName,
            Price = price
        });

        await _dbContext.SaveChangesAsync();

        return item.Entity.Id;
    }

    public async Task<bool?> UpdateAsync(int id, string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
    {
        var item = await _dbContext.CatalogItems.Where(i => i.Id == id).FirstAsync();
        if (item is not null)
        {
            item.Name = name;
            item.Description = description;
            item.Price = price;
            item.AvailableStock = availableStock;
            item.CatalogBrandId = catalogBrandId;
            item.CatalogTypeId = catalogTypeId;
            item.PictureFileName = pictureFileName;
            ////_dbContext.CatalogItems.Update(item);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<bool?> RemoveAsync(int id)
    {
        var item = await _dbContext.CatalogItems.Where(i => i.Id == id).FirstAsync();
        if (item is not null)
        {
            _dbContext.CatalogItems.Remove(item);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<CatalogItem?> GetItemByIdAsync(int id)
    {
        var result = await _dbContext.CatalogItems.Where<CatalogItem>(i => i.Id == id).FirstOrDefaultAsync();
        return result;
    }

    public async Task<SelectedItems<CatalogItem>> GetItemByBrandAsync(string brand)
    {
        var result = await _dbContext.CatalogItems.Where<CatalogItem>(i => i.CatalogBrand.Brand == brand).ToListAsync();

        return new SelectedItems<CatalogItem>() { TotalCount = result.Count, Data = result };
    }

    public async Task<SelectedItems<CatalogItem>> GetItemByTypeAsync(string type)
    {
        var result = await _dbContext.CatalogItems.Where<CatalogItem>(i => i.CatalogType.Type == type).ToListAsync();
        return new SelectedItems<CatalogItem>() { TotalCount = result.Count, Data = result };
    }
}