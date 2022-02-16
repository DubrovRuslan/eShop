using Catalog.Host.Models.Dtos;

namespace Catalog.UnitTests.Services;

public class CatalogTypeServiceTest
{
    private readonly ICatalogTypeService _catalogTypeService;
    private readonly Mock<ICatalogTypeRepository> _catalogTypeRepository;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<CatalogTypeService>> _logger;
    private readonly Mock<IMapper> _mapper;

    private readonly CatalogType _testType = new CatalogType()
    {
        Id = 0,
        Type = "SomeType"
    };

    public CatalogTypeServiceTest()
    {
        _catalogTypeRepository = new Mock<ICatalogTypeRepository>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogTypeService>>();
        _mapper = new Mock<IMapper>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

        _catalogTypeService = new CatalogTypeService(_dbContextWrapper.Object, _logger.Object, _catalogTypeRepository.Object, _mapper.Object);
    }

    [Fact]
    public async Task Add_Success()
    {
        // arrange
        var testResult = 1;

        _catalogTypeRepository.Setup(s => s.AddAsync(It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogTypeService.Add(_testType.Type);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task Addc_Failed()
    {
        // arrange
        int? testResult = null;

        _catalogTypeRepository.Setup(s => s.AddAsync(It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogTypeService.Add(_testType.Type);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task Update_Success()
    {
        // arrange
        var testResult = true;

        _catalogTypeRepository.Setup(s => s.UpdateAsync(
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogTypeService.Update(_testType.Id, _testType.Type);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task Update_Failed()
    {
        // arrange
        bool? testResult = null;

        _catalogTypeRepository.Setup(s => s.UpdateAsync(
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogTypeService.Update(_testType.Id, _testType.Type);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task Remove_Success()
    {
        // arrange
        var testResult = true;

        _catalogTypeRepository.Setup(s => s.RemoveAsync(It.IsAny<int>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogTypeService.Remove(_testType.Id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task Remove_Failed()
    {
        // arrange
        bool? testResult = null;

        _catalogTypeRepository.Setup(s => s.RemoveAsync(It.IsAny<int>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogTypeService.Remove(_testType.Id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task GetCatalogTypeAsync_Success()
    {
        var countTypes = 1;

        // arrange
        var selectedTypeSuccess = new SelectedItems<CatalogType>()
        {
            TotalCount = countTypes,
            Data = new List<CatalogType>()
            {
                new CatalogType()
                {
                    Id = 1,
                    Type = "Some Type"
                }
            }
        };

        var selectedTypeDtoSuccess = new SelectedItems<CatalogTypeDto>()
        {
            TotalCount = countTypes,
            Data = new List<CatalogTypeDto>()
            {
                new CatalogTypeDto()
                {
                    Id = 1,
                    Type = "Some Type"
                }
            }
        };

        var catalogBrend = new CatalogType()
        {
            Id = 1,
            Type = "Some Type"
        };

        var catalogBrendDto = new CatalogTypeDto()
        {
            Id = 1,
            Type = "Some Type"
        };

        _catalogTypeRepository.Setup(s => s.GetTypesAsync()).ReturnsAsync(selectedTypeSuccess);

        _mapper.Setup(s => s.Map<CatalogTypeDto>(
            It.Is<CatalogType>(i => i.Equals(catalogBrend)))).Returns(catalogBrendDto);

        // act
        var result = await _catalogTypeService.GetCatalogTypesAsync();

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(countTypes);
    }

    [Fact]
    public async Task GetCatalogTypeAsync_Failed()
    {
        // arrange
        _catalogTypeRepository.Setup(s => s.GetTypesAsync()).ReturnsAsync((SelectedItems<CatalogType>)null!);

        // act
        var result = await _catalogTypeService.GetCatalogTypesAsync();

        // assert
        result.Should().BeNull();
    }
}