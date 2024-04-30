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
        public async Task GetSizesByProductId_ReturnsEmptyForNonexistentProduct()
        {
            // Arrange
            var nonexistentProductId = Guid.NewGuid(); 

            // Act
            var result = await productSizesService.GetSizesByProductId(nonexistentProductId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
