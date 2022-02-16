using AutoMapper;
using Catalog.Host.Data;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services
{
    public class CatalogTypeService : BaseDataService<ApplicationDbContext>, ICatalogTypeService
    {
        private readonly ICatalogTypeRepository _catalogTypeRepository;
        private readonly IMapper _mapper;
        public CatalogTypeService(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<BaseDataService<ApplicationDbContext>> logger,
            ICatalogTypeRepository catalogTypeRepository,
            IMapper mapper)
            : base(dbContextWrapper, logger)
        {
            _catalogTypeRepository = catalogTypeRepository;
            _mapper = mapper;
        }

        public Task<int?> Add(string type)
        {
            return ExecuteSafeAsync(() => _catalogTypeRepository.AddAsync(type));
        }

        public Task<bool?> Remove(int id)
        {
            return ExecuteSafeAsync(() => _catalogTypeRepository.RemoveAsync(id));
        }

        public Task<bool?> Update(int id, string type)
        {
            return ExecuteSafeAsync(() => _catalogTypeRepository.UpdateAsync(id, type));
        }

        public async Task<SelectedItemsResponse<CatalogTypeDto>> GetCatalogTypesAsync()
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _catalogTypeRepository.GetTypesAsync();
                return new SelectedItemsResponse<CatalogTypeDto>()
                {
                    Count = result.TotalCount,
                    Data = result.Data.Select(s => _mapper.Map<CatalogTypeDto>(s)).ToList()
                };
            });
        }
    }
}
