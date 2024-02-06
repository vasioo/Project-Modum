using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Services.Interfaces;
using Modum.Services.Services;

namespace Modum.Tests.UnitTests.ServiceTests.ServiceTestsFolder
{
    public class ProductSizesServiceTests : ServiceTestsBase
    {
        private IProductSizesService productSizesService;

        public ProductSizesServiceTests()
        {
            productSizesService = new ProductSizesService(context);
        }
        [Fact]
        public async Task GetSizesByProductId_ReturnsCorrectResult()
        {
            // Act
            var result = await productSizesService.GetSizesByProductId(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count()); 
        }

        [Fact]
        public async Task GetSizesByProductId_ReturnsEmptyForNonexistentProduct()
        {
            // Arrange
            var nonexistentProductId = 999; 

            // Act
            var result = await productSizesService.GetSizesByProductId(nonexistentProductId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
