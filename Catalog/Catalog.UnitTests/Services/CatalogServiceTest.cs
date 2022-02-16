using System.Threading;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;
using Moq;

namespace Catalog.UnitTests.Services;

public class CatalogServiceTest
{
    private readonly ICatalogService _catalogService;

    private readonly Mock<ICatalogItemRepository> _catalogItemRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<CatalogService>> _logger;

    private readonly CatalogItemDto _testCatalogItemDto = new CatalogItemDto()
    {
        Id = 1,
        Name = "TestName",
        Description = "TestDescription",
        Price = 100,
        PictureUrl = @"123.jpeg",
        CatalogType = new CatalogTypeDto()
        {
            Id = 1,
            Type = "TestType"
        },
        CatalogBrand = new CatalogBrandDto
        {
            Id = 1,
            Brand = "TestBrand"
        },
        AvailableStock = 12
    };
    private readonly CatalogItem _testCatalogItem = new CatalogItem()
    {
        Id = 1,
        Name = "TestName",
        Description = "TestDescription",
        Price = 100,
        PictureFileName = @"123.jpeg",
        CatalogTypeId = 1,
        CatalogType = new CatalogType()
        {
            Id = 1,
            Type = "TestType"
        },
        CatalogBrandId = 1,
        CatalogBrand = new CatalogBrand
        {
            Id = 1,
            Brand = "TestBrand"
        },
        AvailableStock = 12
    };

    public CatalogServiceTest()
    {
        _catalogItemRepository = new Mock<ICatalogItemRepository>();
        _mapper = new Mock<IMapper>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogService>>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

        _catalogService = new CatalogService(_dbContextWrapper.Object, _logger.Object, _catalogItemRepository.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Success()
    {
        // arrange
        var testPageIndex = 0;
        var testPageSize = 4;
        var testTotalCount = 12;

        var pagingPaginatedItemsSuccess = new PaginatedItems<CatalogItem>()
        {
            Data = new List<CatalogItem>()
            {
                new CatalogItem()
                {
                    Name = "TestName",
                },
            },
            TotalCount = testTotalCount,
        };

        var catalogItemSuccess = new CatalogItem()
        {
            Name = "TestName"
        };

        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Name = "TestName"
        };

        _catalogItemRepository.Setup(s => s.GetByPageAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).ReturnsAsync(pagingPaginatedItemsSuccess);

        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogItemsAsync(testPageSize, testPageIndex);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;

        _catalogItemRepository.Setup(s => s.GetByPageAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).Returns((Func<PaginatedItemsResponse<CatalogItem>>)null!);

        // act
        var result = await _catalogService.GetCatalogItemsAsync(testPageSize, testPageIndex);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCatalogItemByIdAsync_Success()
    {
        var id = 1;
        _catalogItemRepository.Setup(s => s.GetItemByIdAsync(
            It.Is<int>(i => i == id))).ReturnsAsync(_testCatalogItem);
        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(_testCatalogItem)))).Returns(_testCatalogItemDto);
        var result = await _catalogService.GetCatalogItemByIdAsync(id);

        // assert
        result.Should().NotBeNull();
        result?.Name.Should().NotBeNull();
        result?.Id.Should().Be(id);
        result?.Name.Should().Be(_testCatalogItem.Name);
    }

    [Fact]
    public async Task GetCatalogItemByIdAsync_Failed()
    {
        // arrange
        var id = 0;
        _catalogItemRepository.Setup(s => s.GetItemByIdAsync(
            It.Is<int>(i => i == id))).ReturnsAsync((Func<CatalogItem>)null!);

        // act
        var result = await _catalogService.GetCatalogItemByIdAsync(id);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCatalogItemByBrandAsync_Success()
    {
        var selectedItems = new SelectedItems<CatalogItem>()
        {
            TotalCount = 1,
            Data = new List<CatalogItem>() { _testCatalogItem }
        };
        var brand = _testCatalogItem.CatalogBrand.Brand;
        _catalogItemRepository.Setup(s => s.GetItemByBrandAsync(
            It.Is<string>(i => i == brand))).ReturnsAsync(selectedItems);

        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(_testCatalogItem)))).Returns(_testCatalogItemDto);
        var result = await _catalogService.GetCatalogItemByBrandAsync(brand);

        // assert
        result.Should().NotBeNull();
        result.Count.Should().Be(1);
        result.Data.Should().AllBeEquivalentTo(_testCatalogItemDto);
    }

    [Fact]
    public async Task GetCatalogItemByBrandAsync_Failed()
    {
        // arrange
        var brand = string.Empty;
        _catalogItemRepository.Setup(s => s.GetItemByBrandAsync(
            It.Is<string>(i => i == brand))).ReturnsAsync((Func<SelectedItems<CatalogItem>>)null!);

        // act
        var result = await _catalogService.GetCatalogItemByBrandAsync(brand);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCatalogItemByTypeAsync_Success()
    {
        var selectedItems = new SelectedItems<CatalogItem>()
        {
            TotalCount = 1,
            Data = new List<CatalogItem>() { _testCatalogItem }
        };
        var type = _testCatalogItem.CatalogType.Type;
        _catalogItemRepository.Setup(s => s.GetItemByTypeAsync(
            It.Is<string>(i => i == type))).ReturnsAsync(selectedItems);

        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(_testCatalogItem)))).Returns(_testCatalogItemDto);
        var result = await _catalogService.GetCatalogItemByTypeAsync(type);

        // assert
        result.Should().NotBeNull();
        result.Count.Should().Be(1);
        result.Data.Should().AllBeEquivalentTo(_testCatalogItemDto);
    }

    [Fact]
    public async Task GetCatalogItemByTypeAsync_Failed()
    {
        // arrange
        var type = string.Empty;
        _catalogItemRepository.Setup(s => s.GetItemByBrandAsync(
            It.Is<string>(i => i == type))).ReturnsAsync((Func<SelectedItems<CatalogItem>>)null!);

        // act
        var result = await _catalogService.GetCatalogItemByBrandAsync(type);

        // assert
        result.Should().BeNull();
    }
}