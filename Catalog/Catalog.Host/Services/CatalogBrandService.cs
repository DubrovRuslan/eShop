using AutoMapper;
using Catalog.Host.Data;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services
{
    public class CatalogBrandService : BaseDataService<ApplicationDbContext>, ICatalogBrandService
    {
        private readonly ICatalogBrandRepository _catalogBrandRepository;
        private readonly IMapper _mapper;
        public CatalogBrandService(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<BaseDataService<ApplicationDbContext>> logger,
            ICatalogBrandRepository catalogBrandRepository,
            IMapper mapper)
            : base(dbContextWrapper, logger)
        {
            _catalogBrandRepository = catalogBrandRepository;
            _mapper = mapper;
        }

        public Task<int?> Add(string brand)
        {
            return ExecuteSafeAsync(() => _catalogBrandRepository.AddAsync(brand));
        }

        public Task<bool?> Remove(int id)
        {
            return ExecuteSafeAsync(() => _catalogBrandRepository.RemoveAsync(id));
        }

        public Task<bool?> Update(int id, string brand)
        {
            return ExecuteSafeAsync(() => _catalogBrandRepository.UpdateAsync(id, brand));
        }

        public async Task<SelectedItemsResponse<CatalogBrandDto>> GetCatalogBrandsAsync()
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _catalogBrandRepository.GetBrandsAsync();
                return new SelectedItemsResponse<CatalogBrandDto>()
                {
                    Count = result.TotalCount,
                    Data = result.Data.Select(s => _mapper.Map<CatalogBrandDto>(s)).ToList()
                };
            });
        }
    }
}
