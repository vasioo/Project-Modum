using Microsoft.Extensions.Configuration;
using Modum.Models.BaseModels.Models.LTCs;
using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Services.Interfaces;
using Modum.Services.Services;

namespace Modum.Tests.UnitTests.ServiceTests.ServiceTestsFolder
{
    public class ProductServiceTests : ServiceTestsBase
    {
        private IProductService productService;
        private IConfiguration configuration;

        public ProductServiceTests()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddInMemoryCollection();

            configuration = configurationBuilder.Build();
            productService = new ProductService(configuration,context);
        }
        [Fact]
        public async Task GetProductsByMainCategoryAsync_ReturnsCorrectResult()
        {
            // Arrange
            var mainCategoryId = Guid.NewGuid();

            // Act
            var result = await productService.GetProductsByMainCategoryAsync(mainCategoryId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetProductsByPriceAsync_ReturnsCorrectResult()
        {
            // Arrange
            var price = 50.0m;

            // Act
            var result = await productService.GetProductsByPriceAsync(price);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetProductsBySubcategoryAsync_ReturnsCorrectResult()
        {
            // Arrange
            var subcategoryId = Guid.NewGuid();

            // Act
            var result = await productService.GetProductsBySubcategoryAsync(subcategoryId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetProductsByTenMostBought_ReturnsCorrectResult()
        {
            // Act
            var result = await productService.GetProductsByTenMostBought();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetProductsByTenMostAddedToFavourites_ReturnsCorrectResult()
        {
            // Act
            var result = await productService.GetProductsByTenMostAddedToFavourites();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetProductsByFiltersAsync_ReturnsCorrectResult()
        {
            // Arrange
            var filter = new ProductFilter { SearchString = "Bluetooth", MinPrice = 20.0m, MaxPrice = 100.0m };

            // Act
            var result = await productService.GetProductsByFiltersAsync(filter);

            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetProductsByTitleAsync_ReturnsCorrectResult()
        {
            // Arrange
            var title = "Bluetooth Speaker";

            // Act
            var result = await productService.GetProductsByTitleAsync(title);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetProductsByWhoUploadedThemAsync_ReturnsCorrectResult()
        {
            // Arrange
            var username = "User10";

            // Act
            var result = await productService.GetProductsByWhoUploadedThemAsync(username);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetProductsByFiltersAsync_WithEmptyFilter_ReturnsAllProducts()
        {
            // Arrange
            var emptyFilter = new ProductFilter();

            // Act
            var result = await productService.GetProductsByFiltersAsync(emptyFilter);

            // Assert
            Assert.NotNull(result);
        }

    }
}
