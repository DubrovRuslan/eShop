using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;
using Moq;

namespace Catalog.UnitTests.Services;

public class CatalogBrandServiceTest
{
    private readonly ICatalogBrandService _catalogBrandService;

    private readonly Mock<ICatalogBrandRepository> _catalogBrandRepository;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<CatalogBrandService>> _logger;
    private readonly Mock<IMapper> _mapper;

    private readonly CatalogBrand _testBrand = new CatalogBrand()
    {
        Id = 1,
        Brand = "Some brand"
    };

    public CatalogBrandServiceTest()
    {
        _catalogBrandRepository = new Mock<ICatalogBrandRepository>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogBrandService>>();
        _mapper = new Mock<IMapper>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

        _catalogBrandService = new CatalogBrandService(_dbContextWrapper.Object, _logger.Object, _catalogBrandRepository.Object, _mapper.Object);
    }

    [Fact]
    public async Task Add_Success()
    {
        // arrange
        var testResult = 1;

        _catalogBrandRepository.Setup(s => s.AddAsync(It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogBrandService.Add(_testBrand.Brand);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task Add_Failed()
    {
        // arrange
        int? testResult = null;

        _catalogBrandRepository.Setup(s => s.AddAsync(It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogBrandService.Add(_testBrand.Brand);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task Update_Success()
    {
        // arrange
        var testResult = true;

        _catalogBrandRepository.Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogBrandService.Update(_testBrand.Id, _testBrand.Brand);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task Update_Failed()
    {
        // arrange
        bool? testResult = null;

        _catalogBrandRepository.Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogBrandService.Update(_testBrand.Id, _testBrand.Brand);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task Remove_Success()
    {
        // arrange
        var testResult = true;

        _catalogBrandRepository.Setup(s => s.RemoveAsync(It.IsAny<int>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogBrandService.Remove(_testBrand.Id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task Remove_Failed()
    {
        // arrange
        bool? testResult = null;

        _catalogBrandRepository.Setup(s => s.RemoveAsync(It.IsAny<int>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogBrandService.Remove(_testBrand.Id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task GetCatalogBrandAsync_Success()
    {
        var countBrands = 1;

        // arrange
        var selectedBrandSuccess = new SelectedItems<CatalogBrand>()
        {
            TotalCount = countBrands,
            Data = new List<CatalogBrand>()
            {
                new CatalogBrand()
                {
                    Id = 1,
                    Brand = "Some brand"
                }
            }
        };

        var selectedBrandDtoSuccess = new SelectedItems<CatalogBrandDto>()
        {
            TotalCount = countBrands,
            Data = new List<CatalogBrandDto>()
            {
                new CatalogBrandDto()
                {
                    Id = 1,
                    Brand = "Some brand"
                }
            }
        };

        var catalogBrend = new CatalogBrand()
        {
            Id = 1,
            Brand = "Some brand"
        };

        var catalogBrendDto = new CatalogBrandDto()
        {
            Id = 1,
            Brand = "Some brand"
        };

        _catalogBrandRepository.Setup(s => s.GetBrandsAsync()).ReturnsAsync(selectedBrandSuccess);

        _mapper.Setup(s => s.Map<CatalogBrandDto>(
            It.Is<CatalogBrand>(i => i.Equals(catalogBrend)))).Returns(catalogBrendDto);

        // act
        var result = await _catalogBrandService.GetCatalogBrandsAsync();

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(countBrands);
    }

    [Fact]
    public async Task GetCatalogBrandAsync_Failed()
    {
        // arrange
        _catalogBrandRepository.Setup(s => s.GetBrandsAsync()).ReturnsAsync((SelectedItems<CatalogBrand>)null!);

        // act
        var result = await _catalogBrandService.GetCatalogBrandsAsync();

        // assert
        result.Should().BeNull();
    }
}